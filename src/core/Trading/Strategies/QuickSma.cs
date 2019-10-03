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

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
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

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			List<decimal?> smaFast = candles.Sma(preset?.SmaFast ?? 12);
			List<decimal?> smaSlow = candles.Sma(preset?.SmaSlow ?? 18);

			List<decimal> closes = candles.Close();
			List<bool> crossOver = smaFast.Crossover(smaSlow);
			List<bool> crossUnder = smaSlow.Crossunder(closes);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (crossOver[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
				else if (crossUnder[i])
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
				}
			}

			return result;
		}
	}
}
