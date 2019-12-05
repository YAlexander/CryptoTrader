using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using Core.Trading.Extensions;

namespace core.Trading.Strategies
{
	public class AdxMomentum : BaseStrategy
	{
		public override string Name { get; } = "ADX Momentum";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 25;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			AdxMomentumPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonSerializer.Deserialize<AdxMomentumPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> adx = candles.Adx(preset?.Adx ?? 14);
			List<decimal?> diPlus = candles.PlusDi(preset?.PlusDi ?? 25);
			List<decimal?> diMinus = candles.MinusDi(preset?.MinusDi ?? 25);
			List<decimal?> sar = candles.Sar(preset?.AccelerationFactor ?? 0.02, preset?.MaximumAccelerationFactor ?? 0.2);
			List<decimal?> mom = candles.Mom(preset?.Mom ?? 14);

			for (int i = 0; i < candles.Count(); i++)
			{

				if (adx[i] > 25 && mom[i] < 0 && diMinus[i] > 25 && diPlus[i] < diMinus[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else if (adx[i] > 25 && mom[i] > 0 && diPlus[i] > 25 && diPlus[i] > diMinus[i])
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