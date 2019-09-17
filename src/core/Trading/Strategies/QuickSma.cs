using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Extensions;
using core.Trading.Extensions;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using TinyJson;

namespace core.Trading.Strategies
{
	public class QuickSma : BaseStrategy
	{
		public override string Name { get; } = "Quick SMA";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.MINUTE;

		public override int MinNumberOfCandles { get; } = 20;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			QuickSmaPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<QuickSmaPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sma1 = candles.Sma(preset?.SmaFast ?? 12);
			List<decimal?> sma2 = candles.Sma(preset?.SmaSlow ?? 18);

			List<decimal> closes = candles.Close();
			List<bool> crossOver = sma1.Crossover(sma2);
			List<bool> crossUnder = sma2.Crossunder(closes);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (crossOver[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (crossUnder[i])
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
