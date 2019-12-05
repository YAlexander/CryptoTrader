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
	public class RsiOversoldOverbought : BaseStrategy
	{
		public override string Name { get; } = "RSI Oversold/Overbought";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 200;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			RsiOversoldOverboughtPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonSerializer.Deserialize<RsiOversoldOverboughtPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 14);
			List<bool> crossOver = rsi.Crossover(preset?.Low ?? 30);
			List<bool> crossUnder = rsi.Crossunder(preset?.High ?? 70);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else if (crossUnder[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
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
