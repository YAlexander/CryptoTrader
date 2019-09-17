using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using Newtonsoft.Json;

namespace core.Trading.Strategies
{
	public class SmaCrossover : BaseStrategy
	{
		public override string Name { get; } = "SMA Crossover";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 60;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			SmaCrossoverPreset preset = null;
			if (string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonConvert.DeserializeObject<SmaCrossoverPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sma1 = candles.Sma(preset?.Sma1 ?? 12);
			List<decimal?> sma2 = candles.Sma(preset?.Sma2 ?? 16);
			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 14);

			List<bool> crossOver = sma1.Crossover(sma2);
			List<bool> crossUnder = sma1.Crossunder(sma2);

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
					result.Add(TradingAdviceCode.HOLD);
				}
				// When the RSI has dropped 10 points from the peak, sell...
				else if (startRsi - rsi[i] > 10)
				{
					startRsi = 0;
					result.Add(TradingAdviceCode.SELL);
				}
				// When the fast SMA moves above the slow SMA, we have a positive cross-over
				else if (sma1[i] > sma2[i] && sma1[i - 1] < sma2[i - 1] && rsi[i] <= 65)
				{
					startRsi = rsi[i].Value;
					result.Add(TradingAdviceCode.BUY);
				}

				else
				{
					result.Add(TradingAdviceCode.HOLD);
				}
			}

			return result.LastOrDefault();
		}
	}
}
