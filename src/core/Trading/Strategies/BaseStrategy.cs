using System.Collections.Generic;
using core.Abstractions;
using core.Abstractions.TypeCodes;

namespace core.Trading.Strategies
{
	public abstract class BaseStrategy : ITradingStrategy
	{
		public abstract string Name { get; }

		public abstract IPeriodCode OptimalTimeframe { get; }

		public abstract int MinNumberOfCandles { get; }

		public string Preset { get; set; }

		/// <summary>
		/// Values from 0 to 1
		/// </summary>
		public virtual double StrategyWeight { get; set; }

		public abstract ITradingAdviceCode Forecast (IEnumerable<ICandle> candles);
	}
}
