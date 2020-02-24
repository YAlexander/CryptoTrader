using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;
using Core.Helpers;
using Persistence;
using Persistence.Entities;
using Persistence.StrategyOptions.OptionManagers;
using TechanCore.Enums;
using TechanCore.Helpers;

namespace Core.BusinessLogic
{
	public class TradingContextBuilder
	{
		private readonly ICandlesProcessor _candlesProcessor;
		private readonly IStrategyInfoProcessor _strategyInfoProcessor;
		private readonly IEnumerable<IStrategyOptionsManager<IStrategyOption>> _strategyOptionManagers;
		private readonly StrategiesHelper _strategiesHelper;
		private IEnumerable<IRisk> _risks;
		
		public TradingContextBuilder (
			ICandlesProcessor candlesProcessor,
			IStrategyInfoProcessor strategyInfoProcessor,
			IEnumerable<IStrategyOptionsManager<IStrategyOption>> strategyOptionManagers,
			StrategiesHelper strategiesHelper,
			IEnumerable<IRisk> risks)
		{
			_candlesProcessor = candlesProcessor;
			_strategyInfoProcessor = strategyInfoProcessor;
			_strategyOptionManagers = strategyOptionManagers;
			_strategiesHelper = strategiesHelper;
		}

		public async Task<ITradingContext> Build (Exchanges exchangeCode, Assets asset1, Assets asset2)
		{
			ITradingContext context = new TradingContext();
			context.Exchange = exchangeCode;
			context.TradingPair = (asset1, asset2);

			IStrategyInfo strategyInfo = await _strategyInfoProcessor.GetStrategyInfo(exchangeCode, asset1, asset2);

			context.TimeFrame = (Timeframes) strategyInfo.TimeFrame;

			int numberOfCandles = strategyInfo.TimeFrame * context.Strategy.MinNumberOfCandles;
			IEnumerable<Candle> candles = await _candlesProcessor.GetCandles(exchangeCode, asset1, asset2, numberOfCandles);
			ICandle[] groupedCandles = candles.GroupCandles((Timeframes) strategyInfo.TimeFrame);

			if (strategyInfo.UseHeikenAshiCandles)
			{
				groupedCandles = strategyInfo.SmoothHeikenAshiCandles 
					? groupedCandles.HeikenAshiSmoothed(MaTypes.EMA, 14).ToArray() 
					: groupedCandles.HeikenAshi().ToArray();
			}

			context.Candles = groupedCandles; 

			//Get strategy
			IStrategyOptionsManager<IStrategyOption> optionManager = _strategyOptionManagers.SingleOrDefault(x => x.StrategyName.Equals(strategyInfo.Class));
			if (optionManager == null)
			{
				throw new Exception($"Options for strategy {strategyInfo.StrategyName} wasn't found");
			} 
			
			IStrategyOption options = await optionManager.GetOptions(exchangeCode, asset1, asset2, strategyInfo.Class);
			context.Strategy = _strategiesHelper.Get(strategyInfo.Class, options);

			// TODO: get asset balance
			decimal[] balances = Array.Empty<decimal>();
			
			IRiskResult riskResult = null;
			IEnumerable<Task> tasks = _risks.Select(x => new Task(() => x.Get(groupedCandles, strategyInfo, balances, ref riskResult)));
			await Task.WhenAll(tasks);
			context.Risks = riskResult;
			return context;
		}
	}
}
