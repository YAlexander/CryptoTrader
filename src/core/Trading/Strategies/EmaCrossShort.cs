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
	public class EmaCrossShort : BaseStrategy
	{
		public override string Name { get; } = "EMA Cross Short";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.FIVE_MINUTES;

		public override int MinNumberOfCandles { get; } = 36;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			EmaCrossShortPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<EmaCrossShortPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> shorts = candles.Ema(preset?.EmaShort ?? 6);
			List<decimal?> longs = candles.Ema(preset?.EmaLong ?? 3);

			for (int i = 0; i < candles.Count(); i++)
			{
				decimal? diff = 100 * (shorts[i] - longs[i]) / ((shorts[i] + longs[i]) / 2);

				if (diff > 1.5m)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (diff <= -0.1m)
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
