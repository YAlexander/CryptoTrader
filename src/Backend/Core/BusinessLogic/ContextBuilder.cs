﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Contracts.Enums;
using Contracts.Trading;
using Core.Helpers;
using Persistence;
using Persistence.Entities;
using Persistence.StrategyOptions;
using Persistence.StrategyOptions.OptionManagers;

namespace Core.BusinessLogic
{
	public class TradingContextBuilder
	{
		private readonly ICandlesProcessor _candlesProcessor;
		private readonly IStrategyInfoProcessor _strategyInfoProcessor;
		private readonly IEnumerable<IStrategyOptionsManager<IStrategyOption>> _strategyOptionManagers;
		private readonly StrategiesHelper _strategiesHelper;
		
		public TradingContextBuilder (
			ICandlesProcessor candlesProcessor,
			IStrategyInfoProcessor strategyInfoProcessor,
			IEnumerable<IStrategyOptionsManager<IStrategyOption>> strategyOptionManagers,
			StrategiesHelper strategiesHelper)
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

			StrategyInfo strategyInfo = await _strategyInfoProcessor.GetStrategyInfo(exchangeCode, asset1, asset2);

			context.TimeFrame = (Timeframes) strategyInfo.TimeFrame;

			int numberOfCandles = strategyInfo.TimeFrame * context.Strategy.MinNumberOfCandles;
			IEnumerable<Candle> candles = await _candlesProcessor.GetCandles(exchangeCode, asset1, asset2, numberOfCandles);
			context.Candles = candles.GroupCandles((Timeframes) strategyInfo.TimeFrame);

			//Get strategy
			IStrategyOptionsManager<IStrategyOption> optionManager = _strategyOptionManagers.SingleOrDefault(x => x.StrategyName.Equals(strategyInfo.Class));
			if (optionManager == null)
			{
				throw new SmtpException($"Options for strategy {strategyInfo.StrategyName} wasn't found");
			} 
			
			IStrategyOption options = await optionManager.GetOptions(exchangeCode, asset1, asset2, strategyInfo.Class);
			context.Strategy = _strategiesHelper.Get(strategyInfo.Class, options);
			
			// TODO: Add risk management

			return context;
		}
	}
}
