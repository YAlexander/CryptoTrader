using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;

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

		public T GetOptions { get; }

		public virtual TradingAdvices Forecast (ICandle[] candles)
		{
			IEnumerable<(ICandle candle, TradingAdvices forecast)> forecasts = AllForecasts(candles);
			return forecasts.Last().forecast;
		}

		protected abstract IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles);

		protected void Validate(ICandle[] candles, object options)
		{
			if (candles == null || candles.Length < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			if (options == null)
			{
				throw new Exception("Missed strategy's configuration");
			}
		}
	}
}
