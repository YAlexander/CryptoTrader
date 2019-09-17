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
	public class RedWeddingWalder : BaseStrategy
	{
		public override string Name { get; } = "Red Wedding Walder";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.FOUR_HOUR;

		public override int MinNumberOfCandles { get; } = 100;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			RedWeddingPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JSONParser.FromJson<RedWeddingPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();

			decimal fastEnd = 0.666m;
			int smaPeriod = preset?.Sma ?? 6;

			List<decimal?> open = candles.Open().Sma(smaPeriod);
			List<decimal?> close = candles.Close().Sma(smaPeriod);
			List<decimal?> high = candles.High().Sma(smaPeriod);
			List<decimal?> low = candles.Low().Sma(smaPeriod);

			List<decimal> closes = candles.Close();
			List<decimal> highs = candles.High();
			List<decimal> opens = candles.Open();

			// Calculate the vClose
			List<decimal?> vClose = new List<decimal?>();
			for (int i = 0; i < open.Count; i++)
			{
				if (open[i].HasValue && close[i].HasValue && low[i].HasValue && high[i].HasValue)
				{
					vClose.Add((open[i].Value + close[i].Value + low[i].Value + high[i].Value) / 4);
				}
				else
				{
					vClose.Add(null);
				}
			}

			// Calculate the vOpen
			decimal smooth = fastEnd * fastEnd;
			List<decimal?> vOpen = new List<decimal?>();

			for (int i = 0; i < vClose.Count; i++)
			{
				decimal? prev_close = i == 0 ? null : vClose[i - 1];
				decimal? prev_open = i == 0 ? null : vOpen[i - 1];

				if (prev_close == null && prev_open == null)
				{
					vOpen.Add(null);
				}
				else
				{
					vOpen.Add((prev_open == null ? prev_close : prev_open) + smooth * (prev_close - (prev_open == null ? prev_close : prev_open)));
				}
			}

			List<decimal?> snow_high = new List<decimal?>();

			for (int i = 0; i < vClose.Count; i++)
			{
				if (high[i].HasValue && vClose[i].HasValue && vOpen[i].HasValue)
				{
					snow_high.Add(Math.Max(high[i].Value, Math.Max(vClose[i].Value, vOpen[i].Value)));
				}
				else
				{
					snow_high.Add(null);
				}
			}

			List<decimal?> snow_low = new List<decimal?>();

			for (int i = 0; i < vClose.Count; i++)
			{
				if (low[i].HasValue && vClose[i].HasValue && vOpen[i].HasValue)
				{
					snow_low.Add(Math.Min(low[i].Value, Math.Min(vClose[i].Value, vOpen[i].Value)));
				}
				else
				{
					snow_low.Add(null);
				}
			}

			List<decimal?> fish = candles.Fisher(9);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i <= 2)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (fish[i] >= fish[i - 1] && fish[i - 1] >= fish[i - 2] && fish[i - 2] >= fish[i - 3] && closes[i] < snow_low[i] && opens[i] < snow_low[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (closes[i] > snow_high[i])
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
