using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;

namespace Core.Trading.Strategies
{
	public class SmaGoldenCross : BaseStrategy
	{
		public override string Name { get; } = "SMA 50/200 Golden Cross";

		public override int MinNumberOfCandles { get; } = 200;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			Validate(candles, default);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> sma50 = candles.Sma(50);
			List<decimal?> sma200 = candles.Sma(200);
			List<bool> crossUnder = sma50.Crossunder(sma200);
			List<bool> crossOver = sma50.Crossover(sma200);

			for (int i = 0; i < candles.Count(); i++)
			{
				// Since we look back 1 candle, the first candle can never be a signal.
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				// When the slow SMA moves above the fast SMA, we have a negative cross-over
				else if (crossUnder[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				// When the fast SMA moves above the slow SMA, we have a positive cross-over
				else if (crossOver[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
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