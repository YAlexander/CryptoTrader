using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.Extensions;
using Core.Trading.Models;
using Core.Trading.Strategies.StrategyOptions;
using Core.Trading.TAIndicators.Extensions;

namespace Core.Trading.Strategies
{
	public class RedWedding : BaseStrategy
	{
		public override string Name { get; } = "Red Wedding";

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

			List<decimal?> longSma = vClose.Sma(10);
			List<decimal?> shortSma = vClose.Sma(3);
			StochItem stoch = candles.Stoch();
			List<decimal?> fish = candles.Fisher();

			List<bool> smaCrossover = shortSma.Crossover(longSma);
			List<bool> smaCrossunder = shortSma.Crossunder(longSma);
			List<bool> snowCross = vClose.Crossunder(vOpen);

			List<bool> stochCross = stoch.K.Crossunder(80);
			List<bool> stochCross2 = stoch.K.Crossunder(stoch.D);

			for (int i = 0; i < candles.Count(); i++)
			{
				if (i <= 1)
				{
					result.Add((candles.ElementAt(i), TradingAdvices.HOLD));
				}
				else if (fish[i] >= fish[i - 1] && closes[i] < snowHigh[i] && smaCrossover[i])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (fish[i] < fish[i - 1] && fish[i - 1] >= fish[i - 2] || smaCrossunder[i] || snowCross[i] || stochCross[i] || stochCross2[i] && stoch.K[i - 1] > 80)
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
