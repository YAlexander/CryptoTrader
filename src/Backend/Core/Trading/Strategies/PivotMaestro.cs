using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class PivotMaestro : BaseStrategy
	{
		public override string Name { get; } = "Pivot Maestro";

		public override int MinNumberOfCandles { get; } = 10;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> high = candles.PivotHigh(4, 2, false);
			List<decimal?> low = candles.PivotLow(4, 2, false);
			List<decimal> lows = candles.Low();

			for (int i = 0; i < candles.Count(); i++)
			{
				// Buy when a lower pivot was found.
				if (low[i].HasValue)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				// Either a upper pivot or a new potential low pivot should make us sell.
				else if (high[i].HasValue || i > 3 && lows[i] <= lows[i - 1] && lows[i] <= lows[i - 2] && lows[i] <= lows[i - 3] && lows[i] <= lows[i - 4])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
			}

			return result;
		}
	}
}