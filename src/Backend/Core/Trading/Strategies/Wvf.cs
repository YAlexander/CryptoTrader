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
	public class Wvf : BaseStrategy
	{
		public override string Name { get; } = "Williams Vix Fix";

		public override int MinNumberOfCandles { get; } = 40;

		protected override IEnumerable<(ICandle, TradingAdvices)> AllForecasts (ICandle[] candles)
		{
			WvfOptions options = Options.GetOptions<WvfOptions>();
			
			Validate(candles, options);

			List<(ICandle, TradingAdvices)> result = new List<(ICandle, TradingAdvices)>();

			List<decimal?> rsi = candles.Rsi(options?.Rsi ?? 20);
			List<decimal?> ema = rsi.Ema(options?.Ema ?? 10);
			StochItem stochRsi = candles.StochRsi(options?.StochRsi ?? 14);

			List<decimal> wvfs = new List<decimal>();
			List<decimal> standardDevs = new List<decimal>();
			List<decimal> rangeHighs = new List<decimal>();
			List<decimal?> upperRanges = new List<decimal?>();

			int pd = 22; // LookBack Period Standard Deviation High
			int bbl = 20; // Bollinger Band Length
			decimal mult = 2.0m; // Bollinger Band Standard Deviation Up
			int lb = 50; // Look Back Period Percentile High
			decimal ph = .85m; // Highest Percentile - 0.90=90%, 0.95=95%, 0.99=99%

			for (int i = 0; i < candles.Count(); i++)
			{
				int itemsToPick = i < pd - 1 ? i + 1 : pd;
				int indexToStartFrom = i < pd - 1 ? 0 : i - pd;

				decimal highestClose = candles.Skip(indexToStartFrom).Take(itemsToPick).Select(x => x.Close).Max();
				decimal wvf = (highestClose - candles.ElementAt(i).Low) / highestClose * 100;

				// Calculate the WVF
				wvfs.Add(wvf);

				decimal standardDev = 0;

				if (wvfs.Count > 1)
				{
					if (wvfs.Count < bbl)
					{
						standardDev = mult * GetStandardDeviation(wvfs.Take(bbl).ToList());
					}
					else
					{
						standardDev = mult * GetStandardDeviation(wvfs.Skip(wvfs.Count - bbl).Take(bbl).ToList());
					}
				}

				// Also calculate the standard deviation.
				standardDevs.Add(standardDev);
			}

			List<decimal?> midLines = wvfs.Sma(bbl);

			for (int i = 0; i < candles.Count(); i++)
			{
				decimal? upperBand = midLines[i] + standardDevs[i];

				if (upperBand.HasValue)
				{
					upperRanges.Add(upperBand.Value);
				}
				else
				{
					upperRanges.Add(null);
				}

				int itemsToPickRange = i < lb - 1 ? i + 1 : lb;
				int indexToStartFromRange = i < lb - 1 ? 0 : i - lb;

				decimal rangeHigh = wvfs.Skip(indexToStartFromRange).Take(itemsToPickRange).Max() * ph;
				rangeHighs.Add(rangeHigh);
			}

			for (int i = 0; i < candles.Count(); i++)
			{
				if (wvfs[i] >= upperRanges[i] || wvfs[i] >= rangeHighs[i] && rsi[i] > ema[i] && rsi[i - 1] < ema[i - 1])
				{
					result.Add((candles.ElementAt(i), TradingAdvices.BUY));
				}
				else if (stochRsi.K[i] > 80 && stochRsi.K[i] > stochRsi.D[i] && stochRsi.K[i - 1] < stochRsi.D[i - 1])
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

		private decimal GetStandardDeviation (List<decimal> doubleList)
		{
			decimal average = doubleList.Average();
			decimal sumOfDerivation = 0;
			foreach (decimal value in doubleList)
			{
				sumOfDerivation += value * value;
			}
			decimal sumOfDerivationAverage = sumOfDerivation / (doubleList.Count - 1);
			return (decimal)Math.Sqrt((double)(sumOfDerivationAverage - average * average));
		}
	}
}