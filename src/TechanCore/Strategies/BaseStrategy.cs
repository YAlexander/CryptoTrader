using System;
using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Strategies.Options;

namespace TechanCore.Strategies
{
	public abstract class BaseStrategy<T> : ITradingStrategy<T> where T : IStrategyOption
	{
		protected BaseStrategy(T options)
		{
			GetOptions = options;
		}
		
		public abstract string Name { get; }
		
		public abstract int MinNumberOfCandles { get; }

		protected List<(ICandle, TradingAdvices)> Result { get; } = new List<(ICandle, TradingAdvices)>();

		public T GetOptions { get; }

		public virtual TradingAdvices Forecast (ICandle[] candles, IOrdersBook ordersBook = null)
		{
			IEnumerable<(ICandle candle, TradingAdvices forecast)> forecasts = AllForecasts(candles, ordersBook);
			return forecasts.Last().forecast;
		}

		protected abstract IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles, IOrdersBook ordersBook = null);

		protected void Validate(ICandle[] candles, T options)
		{
			if (candles == null || candles.Length < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			if (typeof(T) != typeof(EmptyStrategyOptions) && options == null)
			{
				throw new Exception("Missed strategy's configuration");
			}
		}
	}
}
