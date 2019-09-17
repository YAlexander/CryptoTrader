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
	public class EmaAdx : BaseStrategy
	{
		public override string Name { get; } = "EMA ADX";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 36;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}
			EmaAdxPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<EmaAdxPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> emaFast = candles.Ema(preset?.EmaFast ?? 12);
			List<decimal?> emaShort = candles.Ema(preset?.EmaSlow ?? 36);
			List<decimal?> adx = candles.Adx(preset?.Adx);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (emaFast[i] > emaShort[i] && emaFast[i - 1] < emaShort[i] && adx[i] < 20)
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else if (emaFast[i] < emaShort[i] && emaFast[i - 1] > emaShort[i] && adx[i] >= 20)
				{
					result.Add(TradingAdviceCode.BUY);
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
