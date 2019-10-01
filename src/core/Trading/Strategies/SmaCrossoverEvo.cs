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
	public class SmaCrossoverEvo : BaseStrategy
	{
		public override string Name { get; } = "SMA Crossover EVO";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.QUARTER_HOUR;

		public override int MinNumberOfCandles { get; } = 60;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			SmaCrossoverEvoPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonConvert.DeserializeObject<SmaCrossoverEvoPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> shorts = candles.Ema(preset?.Ema1 ?? 6);
			List<decimal?> longs = candles.Ema(preset?.Ema2 ?? 3);

			for (int i = 0; i < candles.Count(); i++)
			{
				var diff = 100 * (shorts[i] - longs[i]) / ((shorts[i] + longs[i]) / 2);

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
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
			}
			return result;
		}
	}
}
