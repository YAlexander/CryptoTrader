using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using TinyJson;

namespace core.Trading.Strategies
{
	public class SmaGoldenCross : BaseStrategy
	{
		public override string Name { get; } = "SMA 50/200 Golden Cross";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 200;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			SmaGoldenCrossPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<SmaGoldenCrossPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> sma50 = candles.Sma(preset?.SmaFast ?? 50);
			List<decimal?> sma200 = candles.Sma(preset?.SmaSlow ?? 200);
			List<bool> crossUnder = sma50.Crossunder(sma200);
			List<bool> crossOver = sma50.Crossover(sma200);

			for (int i = 0; i < candles.Count(); i++)
			{
				// Since we look back 1 candle, the first candle can never be a signal.
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				// When the slow SMA moves above the fast SMA, we have a negative cross-over
				else if (crossUnder[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				// When the fast SMA moves above the slow SMA, we have a positive cross-over
				else if (crossOver[i])
				{
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