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
	public class FisherTransform : BaseStrategy
	{
		public override string Name { get; } = "Fisher Transform";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 40;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			FisherTransformPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonSerializer.Deserialize<FisherTransformPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> fishers = candles.Fisher(preset?.Fisher ?? 10);
			List<decimal?> ao = candles.AwesomeOscillator();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (fishers[i] < 0 && fishers[i - 1] > 0 && ao[i] < 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (fishers[i] == 1)
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