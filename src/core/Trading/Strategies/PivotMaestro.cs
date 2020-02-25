using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Extensions;
using core.Trading.Extensions;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class PivotMaestro : BaseStrategy
	{
		public override string Name { get; } = "Pivot Maestro";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.QUARTER_HOUR;

		public override int MinNumberOfCandles { get; } = 10;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> high = candles.PivotHigh(4, 2, false);
			List<decimal?> low = candles.PivotLow(4, 2, false);
			List<decimal> lows = candles.Low();

			for (int i = 0; i < candles.Count(); i++)
			{
				// Buy when a lower pivot was found.
				if (low[i].HasValue)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				// Either a upper pivot or a new potential low pivot should make us sell.
				else if (high[i].HasValue || i > 3 && lows[i] <= lows[i - 1] && lows[i] <= lows[i - 2] && lows[i] <= lows[i - 3] && lows[i] <= lows[i - 4])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
			}

			return result;
		}
	}
}