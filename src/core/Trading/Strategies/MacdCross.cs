using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using TinyJson;

namespace core.Trading.Strategies
{
	public class MacdCross : BaseStrategy
	{
		public override string Name { get; } = "MACD X";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 50;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			MacdCrossPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<MacdCrossPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			MacdItem macd = candles.Macd(preset?.MacdFast, preset?.MacdSlow, preset?.MacdSignal);
			for (int i = 0; i < candles.Count(); i++)
			{
				var crossUnder = macd.Macd.Crossunder(macd.Signal);
				var crossOver = macd.Macd.Crossover(macd.Signal);
				if (i == 0)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (macd.Macd[i] > 0 && crossUnder[i])
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else if (macd.Macd[i] < 0 && crossOver[i])
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
