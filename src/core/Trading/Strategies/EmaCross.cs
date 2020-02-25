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
	public class EmaCross : BaseStrategy
	{
		public override string Name { get; } = "EMA Cross";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 36;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			EmaCrossPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonConvert.DeserializeObject<EmaCrossPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> emaShort = candles.Ema(preset?.Ema1 ?? 12);
			List<decimal?> emaLong = candles.Ema(preset?.Ema2 ?? 26);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (emaShort[i] < emaLong[i] && emaShort[i - 1] > emaLong[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (emaShort[i] > emaLong[i] && emaShort[i - 1] < emaLong[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
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
