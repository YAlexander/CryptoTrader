using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class BullishEngulfing : BaseStrategy
	{
		public override string Name { get; } = "Bullish Engulfing";

		public override int MinNumberOfCandles { get; } = 11;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> rsi = candles.Rsi(11);
			List<decimal> close = candles.Select(x => x.Close).ToList();
			List<decimal> high = candles.Select(x => x.High).ToList();
			List<decimal> low = candles.Select(x => x.Low).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (rsi[i] < 40 && low[i - 1] < close[i] && high[i - 1] < high[i] && high[i - 1] < close[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (high[i - 1] > close[i] && low[i - 1] < low[i] && close[i - 1] < low[i] && rsi[i] > 60)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}
