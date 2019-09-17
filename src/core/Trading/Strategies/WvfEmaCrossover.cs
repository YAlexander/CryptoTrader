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
	public class WvfEmaCrossover : BaseStrategy
	{
		public override string Name { get; } = "Wvf Ema Crossover";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.QUARTER_HOUR;

		public override int MinNumberOfCandles { get; } = 40;

		public override ITradingAdviceCode Forecast (IEnumerable<ICandle> candleItems)
		{
			if (candleItems.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			WvfEmaCrossoverPreset preset = null;
			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonConvert.DeserializeObject<WvfEmaCrossoverPreset>(Preset);
			}

			List<TradingAdviceCode> result = new List<TradingAdviceCode>();
			List<ICandle> candles = candleItems.ToList();

			List<decimal> close = candles.Select(x => x.Close).ToList();
			List<decimal> high = candles.Select(x => x.High).ToList();
			List<decimal> low = candles.Select(x => x.Low).ToList();
			List<decimal> open = candles.Select(x => x.Open).ToList();

			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 20);
			List<decimal?> ema = rsi.Ema(preset?.Ema ??10);
			StochItem stochRsi = candles.StochRsi(preset?.StochRsi ?? 14);

			List<decimal> wvfs = new List<decimal>();
			List<decimal> standardDevs = new List<decimal>();
			List<decimal> rangeHighs = new List<decimal>();
			List<decimal?> upperRanges = new List<decimal?>();

			int pd = 22; // LookBack Period Standard Deviation High
			int bbl = 20; // Bollinger Band Length
			decimal mult = 2.0m; // Bollinger Band Standard Deviation Up
			int lb = 50; // Look Back Period Percentile High
			decimal ph = .85m; // Highest Percentile - 0.90=90%, 0.95=95%, 0.99=99%

			List<decimal?> shorts = candles.Ema(6);
			List<decimal?> longs = candles.Ema(3);

			for (int i = 0; i < candles.Count(); i++)
			{
				int itemsToPick = i < pd - 1 ? i + 1 : pd;
				int indexToStartFrom = i < pd - 1 ? 0 : i - pd;

				decimal highestClose = candles.Skip(indexToStartFrom).Take(itemsToPick).Select(x => x.Close).Max();
				decimal wvf = (highestClose - candles[i].Low) / highestClose * 100;

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

			for (int i = 0; i < candles.Count; i++)
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
				double diff = Convert.ToDouble(100 * (shorts[i] - longs[i]) / ((shorts[i] + longs[i]) / 2));

				if (diff > 1.5 && (wvfs[i] >= upperRanges[i] || wvfs[i] >= rangeHighs[i] && rsi[i] > ema[i] && rsi[i - 1] < ema[i - 1]))
				{
					result.Add(TradingAdviceCode.BUY);
				}
				else if (diff <= -0.1 && stochRsi.K[i] > 80 && stochRsi.K[i] > stochRsi.D[i] && stochRsi.K[i - 1] < stochRsi.D[i - 1])
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