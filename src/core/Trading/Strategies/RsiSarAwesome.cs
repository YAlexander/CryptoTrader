using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using Core.Trading.Extensions;
using TinyJson;

namespace core.Trading.Strategies
{
	public class RsiSarAwesome : BaseStrategy
	{
		public override string Name { get; } = "RSI SAR Awesome";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 35;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			RsiSarAwesomePreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<RsiSarAwesomePreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			List<decimal?> sar = candles.Sar();
			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 5);
			List<decimal?> ao = candles.AwesomeOscillator();

			List<decimal> close = candles.Select(x => x.Close).ToList();

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i >= 2)
				{
					decimal? currentSar = sar[i];
					decimal? priorSar = sar[i - 1];

					if (currentSar < close[i] && priorSar > close[i] && ao[i] > 0 && rsi[i] > 50)
					{
						result.Add(TradingAdviceCode.BUY);
					}
					else if (currentSar > close[i] && priorSar < close[i] && ao[i] < 0 && rsi[i] < 50)
					{
						result.Add(TradingAdviceCode.SELL);
					}
					else
					{
						result.Add(TradingAdviceCode.HOLD);
					}
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
