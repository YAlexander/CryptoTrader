using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class SmaCrossover : BaseStrategy
	{
		public override string Name { get; } = "SMA Crossover";

		public override int MinNumberOfCandles { get; } = 60;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			SmaCrossoverOptions options = Options.GetOptions<SmaCrossoverOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> smaFast = candles.Sma(options?.Sma1 ?? 12);
			List<decimal?> smaSlow = candles.Sma(options?.Sma2 ?? 16);
			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 14);

			// TODO: Replace conditions with crossover extensions
			//List<bool> crossOver = smaFast.Crossover(smaSlow);
			//List<bool> crossUnder = smaFast.Crossunder(smaSlow);

			decimal startRsi = 0m;

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] > startRsi)
				{
					startRsi = rsi[i].Value;
				}

				// Since we look back 1 candle, the first candle can never be a signal.
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				// When the RSI has dropped 10 points from the peak, sell...
				else if (startRsi - rsi[i] > 10)
				{
					startRsi = 0;
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				// When the fast SMA moves above the slow SMA, we have a positive cross-over
				else if (smaFast[i] > smaSlow[i] && smaFast[i - 1] < smaSlow[i - 1] && rsi[i] <= 65)
				{
					startRsi = rsi[i].Value;
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
