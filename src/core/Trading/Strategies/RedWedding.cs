using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Extensions;
using core.Trading.Extensions;
using core.Trading.Models;
using core.Trading.Strategies.Presets;
using core.TypeCodes;
using TinyJson;

namespace core.Trading.Strategies
{
	public class RedWedding : BaseStrategy
	{
		public override string Name { get; } = "Red Wedding";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 100;

		//private decimal _lastValue = 0;

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
			List<decimal?> shiftedvClose = new List<decimal?> { null };

			foreach (decimal? item in vClose)
			{
				if (item != vClose.Last())
				{
					shiftedvClose.Add(item);
				}
			}

			for (int i = 0; i < vClose.Count; i++)
			{
				if (shiftedvClose[i] != null)
				{
					if (vClose[i] == null)
					{
						vOpen.Add(shiftedvClose[i]);
					}
					else if (vOpen[i - 1] == null)
					{
						vOpen.Add(shiftedvClose[i]);
					}
					else
					{
						vOpen.Add(vOpen[i - 1] + smooth * (shiftedvClose[i] - vOpen[i - 1]));
					}
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

			List<decimal?> long_sma = vClose.Sma(10);
			List<decimal?> short_sma = vClose.Sma(3);
			StochItem stoch = candles.Stoch();
			List<decimal?> fish = candles.Fisher();

			List<bool> sma_crossover = short_sma.Crossover(long_sma);
			List<bool> sma_crossunder = short_sma.Crossunder(long_sma);
			List<bool> snow_cross = vClose.Crossunder(vOpen);

			List<bool> stoch_cross = stoch.K.Crossunder(80);
			List<bool> stoch_cross2 = stoch.K.Crossunder(stoch.D);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i <= 1)
				{
					result.Add(TradingAdviceCode.HOLD);
				}
				else if (fish[i] >= fish[i - 1] && closes[i] < snow_high[i] && sma_crossover[i])
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (fish[i] < fish[i - 1] && fish[i - 1] >= fish[i - 2] || sma_crossunder[i] || snow_cross[i] || stoch_cross[i] || stoch_cross2[i] && stoch.K[i - 1] > 80)
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
