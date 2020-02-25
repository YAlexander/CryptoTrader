using core.Abstractions.TypeCodes;
using System.Collections.Generic;

namespace core.Abstractions
{
	public interface ITradingStrategy
	{
		string Name { get; }

		double StrategyWeight { get; set; }

		IPeriodCode OptimalTimeframe { get; }

		int MinNumberOfCandles { get; }

		string Preset { get; set; }

		ITradingAdviceCode Forecast (IEnumerable<ICandle> candles);
	}
}
