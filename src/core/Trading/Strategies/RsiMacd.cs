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
	public class RsiMacd : BaseStrategy
	{
		public override string Name { get; } = "RSI MACD";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 52;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			RsiMacdPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<RsiMacdPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			MacdItem macd = candles.Macd(preset?.MacdFast ?? 24, preset?.MacdSlow ?? 52, preset?.MacdSignal ?? 18);
			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] > 70 && macd.Macd[i] - macd.Signal[i] < 0)
				{
					result.Add(TradingAdviceCode.SELL);
				}
				else if (rsi[i] < 30 && macd.Macd[i] - macd.Signal[i] > 0)
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
