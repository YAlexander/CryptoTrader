using core.Abstractions;
using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using core.Trading;
using core.TypeCodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class TradingContextBuilder : BaseProcessor
	{
		private IOptions<AppSettings> _settings;
		private ICandleManager _candlesManager;
		private IStrategyManager _strategyManager;

		public TradingContext Context;

		public TradingContextBuilder (
			IOptions<AppSettings> settings,
			ILogger<TradingContextBuilder> logger,
			ICandleManager candlesManager,
			IStrategyManager strategyManager) : base(settings, logger)
		{
			_settings = settings;
			_candlesManager = candlesManager;
			_strategyManager = strategyManager;
		}

		public async Task<TradingContext> Build (ExchangeCode exchangeCode, string symbol)
		{
			ITradingStrategy strategy = null;
			IEnumerable<ICandle> candles = null;

			return await WithConnection(async (connection, transaction) =>
			{
				Strategy strategyInfo = await _strategyManager.Get(exchangeCode, symbol, connection);

				if (!strategyInfo.IsEnabled)
				{
					return null;
				}

				Type type = Type.GetType(strategyInfo.TypeName, true, true);

				strategy = (ITradingStrategy)Activator.CreateInstance(type);

				// TODO: Add Strategy preset support
				//strategy.Preset = strategyInfo.Preset;

				candles = await _candlesManager.GetLastCandles(exchangeCode, symbol, strategy.OptimalTimeframe.Code, strategy.MinNumberOfCandles, connection, transaction);
				
				TradingContext context = new TradingContext(exchangeCode, symbol, strategy, candles);
				return context;
			});
		}
	}
}
