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
	public class RsiMacdMfi : BaseStrategy
	{
		public override string Name { get; } = "RSI MACD MFI";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 35;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			RsiMacdMfiPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<RsiMacdMfiPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			MacdItem macd = candles.Macd(preset?.MacdFast ?? 5, preset?.MacdSlow ?? 10, preset?.MacdSignal ?? 4);
			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 16);
			List<decimal?> mfi = candles.Mfi(preset?.Mfi);
			List<decimal?> ao = candles.AwesomeOscillator();

			List<decimal> close = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (mfi[i] < 30 && rsi[i] < 45 && ao[i] > 0)
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (mfi[i] > 30 && rsi[i] > 45 && ao[i] < 0)
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