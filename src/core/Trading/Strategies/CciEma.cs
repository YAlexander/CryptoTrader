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
	public class CciEma : BaseStrategy
	{
		public override string Name { get; } = "CCI EMA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 30;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			CciEmaPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<CciEmaPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> cci = candles.Cci(preset?.Cci ?? 30);
			List<decimal?> emaFast = candles.Ema(preset?.EmaFast ?? 8);
			List<decimal?> emaSlow = candles.Ema(preset?.EmaSlow ?? 28);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (cci[i] < -100 && emaFast[i] > emaSlow[i] && emaFast[i - 1] < emaSlow[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (cci[i] > 100 && emaFast[i] < emaSlow[i] && emaFast[i - 1] > emaSlow[i])
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
