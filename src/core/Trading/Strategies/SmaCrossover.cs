using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Strategies.Presets;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class SmaCrossover : BaseStrategy
	{
		public override string Name { get; } = "SMA Crossover";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 60;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			SmaCrossoverPreset preset = null;
			if (string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonSerializer.Deserialize<SmaCrossoverPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> smaFast = candles.Sma(preset?.Sma1 ?? 12);
			List<decimal?> smaSlow = candles.Sma(preset?.Sma2 ?? 16);
			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 14);

			List<bool> crossOver = smaFast.Crossover(smaSlow);
			List<bool> crossUnder = smaFast.Crossunder(smaSlow);

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
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				// When the RSI has dropped 10 points from the peak, sell...
				else if (startRsi - rsi[i] > 10)
				{
					startRsi = 0;
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				// When the fast SMA moves above the slow SMA, we have a positive cross-over
				else if (smaFast[i] > smaSlow[i] && smaFast[i - 1] < smaSlow[i - 1] && rsi[i] <= 65)
				{
					startRsi = rsi[i].Value;
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}

				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}

			return result;
		}
	}
}
