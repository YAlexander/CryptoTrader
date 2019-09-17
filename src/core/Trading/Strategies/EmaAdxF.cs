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
	public class EmaAdxF : BaseStrategy
	{
		public override string Name { get; } = "EMA ADX F";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 15;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			EmaAdxFPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<EmaAdxFPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal> closes = candles.Select(x => x.Close).ToList();
			List<decimal?> ema = candles.Ema(preset?.Ema ?? 9);
			List<decimal?> adx = candles.Adx(preset?.Adx ?? 14);
			List<decimal?> minusDi = candles.MinusDi(preset?.MinusDi ?? 14);
			List<decimal?> plusDi = candles.PlusDi(preset?.PlusDi ?? 14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (ema[i] < closes[i] && plusDi[i] > 20 && plusDi[i] > minusDi[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (ema[i] > closes[i] && minusDi[i] > 20 && plusDi[i] < minusDi[i])
				{
					result.Add(TradingAdviceCode.SELL);
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
