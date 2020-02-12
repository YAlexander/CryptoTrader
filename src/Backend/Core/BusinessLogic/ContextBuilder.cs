using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Contracts.Trading;
using Core.Helpers;
using core.Trading;
using Persistence;
using Persistence.Entities;
using Persistence.StrategyOptions;

namespace Core.BusinessLogic
{
	public class TradingContextBuilder
	{
		private readonly ICandlesProcessor _candlesProcessor;
		private readonly IStrategyInfoProcessor _strategyInfoProcessor;
		private readonly IStrategyOptionsManager _strategyOptionsProcessor;
		
		public TradingContextBuilder (
			ICandlesProcessor candlesProcessor,
			IStrategyInfoProcessor strategyInfoProcessor,
			IStrategyOptionsManager strategyOptionsProcessor)
		{
			_candlesProcessor = candlesProcessor;
			_strategyInfoProcessor = strategyInfoProcessor;
			_strategyOptionsProcessor = strategyOptionsProcessor;
		}

		public async Task<ITradingContext> Build (Exchanges exchangeCode, Assets asset1, Assets asset2)
		{
			ITradingContext context = new TradingContext();
			context.Exchange = exchangeCode;
			context.TradingPair = (asset1, asset2);

			StrategyInfo strategyInfo = await _strategyInfoProcessor.GetStrategyInfo(exchangeCode, asset1, asset2);
			context.Strategy = StrategiesHelper.Get(strategyInfo.StrategyName);
			context.TimeFrame = (Timeframes) strategyInfo.TimeFrame;
			
			context.Strategy.Options = new AdxMomentumOptionsProcessor(IStrategyOptionsManager, exchangeCode, asset1, asset2, strategyInfo.StrategyName); 

			int numberOfCandles = strategyInfo.TimeFrame * context.Strategy.MinNumberOfCandles;
			IEnumerable<Candle> candles = await _candlesProcessor.GetCandles(exchangeCode, asset1, asset2, numberOfCandles);
			context.Candles = candles.GroupCandles((Timeframes) strategyInfo.TimeFrame);
			
			// TODO: Add risk management

			return context;
		}
	}
}
