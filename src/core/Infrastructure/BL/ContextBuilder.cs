using core.Abstractions;
using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.Trading;
using core.TypeCodes;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class TradingContextBuilder
	{
		private IOptions<AppSettings> _settings;
		private ICandleManager _candlesManager;
		private IStrategyManager _strategyManager;

		public TradingContext Context;

		public TradingContextBuilder (IOptions<AppSettings> settings, ICandleManager candlesManager, IStrategyManager strategyManager)
		{
			_settings = settings;
			_candlesManager = candlesManager;
			_strategyManager = strategyManager;
		}

		public async Task<TradingContext> Build (ExchangeCode exchangeCode, string symbol)
		{
			ITradingStrategy strategy = null;
			IEnumerable<ICandle> candles = null;

			using (IDbConnection connection = new NpgsqlConnection(_settings.Value.ConnectionString))
			{
				Strategy strategyInfo = await _strategyManager.Get(exchangeCode, symbol, connection);

				if (!strategyInfo.IsEnabled)
				{
					return null;
				}

				Type type = Type.GetType(strategyInfo.TypeName, true, true);

				strategy = (ITradingStrategy)Activator.CreateInstance(type);
				strategy.Preset = strategyInfo.Preset;

				int numberOfCandles = strategy.MinNumberOfCandles * strategy.OptimalTimeframe.Code;

				// TODO: Use timeframe 1 for test
				//candles = await _candlesManager.GetLastCandles(exchangeCode, symbol, strategyInfo.OptimalTimeframe, numberOfCandles, connection, transaction);
				candles = await _candlesManager.GetLastCandles(exchangeCode, symbol, 1, numberOfCandles, connection);
			}

			TradingContext context = new TradingContext(exchangeCode, symbol, strategy, candles);
			return context;
		}
	}
}
