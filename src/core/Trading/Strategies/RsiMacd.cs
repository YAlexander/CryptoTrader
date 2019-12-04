using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using Newtonsoft.Json;

namespace core.Trading.Strategies
{
	public class RsiMacd : BaseStrategy
	{
		public override string Name { get; } = "RSI MACD";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 52;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			RsiMacdPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonConvert.DeserializeObject<RsiMacdPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();

			MacdItem macd = candles.Macd(preset?.MacdFast ?? 12, preset?.MacdSlow ?? 26, preset?.MacdSignal ?? 9);
			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 14);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (rsi[i] > 70 && macd.Macd[i] - macd.Signal[i] < 0)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
				}
				else if (rsi[i] < 30 && macd.Macd[i] - macd.Signal[i] > 0)
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
