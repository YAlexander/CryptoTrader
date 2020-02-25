using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.TypeCodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.Trading
{
	public class TradingContext
	{
		private IExchangeCode _exchange { get; set; } = ExchangeCode.UNKNOWN;

		private string _symbol;

		private ITradingStrategy _strategy;

		private IEnumerable<ICandle> _candles;

		public TradingContext(IExchangeCode exchange, string symbol, ITradingStrategy strategy, IEnumerable<ICandle> candles)
		{
			_exchange = exchange;
			_symbol = symbol;
			_strategy = strategy;
			_candles = candles;
		}

		public ITradingAdviceCode GetForecast ()
		{
			if(_candles.Count() < _strategy.MinNumberOfCandles)
			{
				throw new Exception($"{_exchange.Description} - {_symbol}: Number of candles is not enough for strategy {_strategy.Name}");
			}

			return _strategy.Forecast(_candles);
		}
	}
}
