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
	public class BreakoutMa : BaseStrategy
	{
		public override string Name { get; } = "Breakout MA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 35;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			ICandleVariableCode defaultVariableCode = CandleVariableCode.LOW;

			BreakoutMaPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonSerializer.Deserialize<BreakoutMaPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> sma20 = candles.Sma(preset?.Sma ?? 20, preset != null ? CandleVariableCode.Create(preset.SmaCandleVariableCode) : defaultVariableCode);
			List<decimal?> ema34 = candles.Ema(preset?.Ema ?? 34);
			List<decimal?> adx = candles.Adx(preset?.Adx ?? 13);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (ema34[i - 1] > sma20[i - 1] && ema34[i] < sma20[i] && adx[i] > 25)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (ema34[i] > sma20[i] && ema34[i - 1] < sma20[i - 1] && adx[i] > 25)
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
