using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Strategies.StrategyOptions;

namespace Core.Trading.Strategies
{
	public class RedWeddingWalder : BaseStrategy
	{
		public override string Name { get; } = "Red Wedding Walder";
		
		public override int MinNumberOfCandles { get; } = 100;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			RedWeddingOptions options = Options.GetOptions<RedWeddingOptions>();

			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			decimal fastEnd = 0.666m;
			int smaPeriod = options?.Sma ?? 6;

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
				decimal? prevClose = i == 0 ? null : vClose[i - 1];
				decimal? prevOpen = i == 0 ? null : vOpen[i - 1];

				if (prevClose == null && prevOpen == null)
				{
					vOpen.Add(null);
				}
				else
				{
					vOpen.Add((prevOpen ?? prevClose) + smooth * (prevClose - (prevOpen ?? prevClose)));
				}
			}

			List<decimal?> snowHigh = new List<decimal?>();

			for (int i = 0; i < vClose.Count; i++)
			{
				if (high[i].HasValue && vClose[i].HasValue && vOpen[i].HasValue)
				{
					snowHigh.Add(Math.Max(high[i].Value, Math.Max(vClose[i].Value, vOpen[i].Value)));
				}
				else
				{
					snowHigh.Add(null);
				}
			}

			List<decimal?> snowLow = new List<decimal?>();

			for (int i = 0; i < vClose.Count; i++)
			{
				if (low[i].HasValue && vClose[i].HasValue && vOpen[i].HasValue)
				{
					snowLow.Add(Math.Min(low[i].Value, Math.Min(vClose[i].Value, vOpen[i].Value)));
				}
				else
				{
					snowLow.Add(null);
				}
			}

			List<decimal?> fish = candles.Fisher(9);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i <= 2)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (fish[i] >= fish[i - 1] && fish[i - 1] >= fish[i - 2] && fish[i - 2] >= fish[i - 3] && closes[i] < snowLow[i] && opens[i] < snowLow[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (closes[i] > snowHigh[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.SELL));
				}
				else
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
			}

			return result;
		}
	}
}
