using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Extensions;
using core.Trading.Models;
using core.Trading.Strategies.Presets;
using core.TypeCodes;

namespace core.Trading.Strategies
{
	public class WvfExtended : BaseStrategy
	{
		public override string Name { get; } = "Williams Vix Fix (Extended)";

		public override IPeriodCode OptimalTimeframe { get; } = PeriodCode.HOUR;

		public override int MinNumberOfCandles { get; } = 40;

		public override IEnumerable<(ICandle, ITradingAdviceCode)> AllForecasts (IEnumerable<ICandle> candles)
		{
			if (candles.Count() < MinNumberOfCandles)
			{
				throw new Exception("Number of candles less then expected");
			}

			WvfExtendedPreset preset = null;

			if (!string.IsNullOrWhiteSpace(Preset))
			{
				preset = JsonSerializer.Deserialize<WvfExtendedPreset>(Preset);
			}

			List<(ICandle, ITradingAdviceCode)> result = new List<(ICandle, ITradingAdviceCode)>();
			//List<ICandle> candles = candleItems.ToList();

			List<decimal> close = candles.Select(x => x.Close).ToList();
			List<decimal> high = candles.Select(x => x.High).ToList();
			List<decimal> low = candles.Select(x => x.Low).ToList();
			List<decimal> open = candles.Select(x => x.Open).ToList();

			List<decimal?> rsi = candles.Rsi(preset?.Rsi ?? 20);
			List<decimal?> ema = rsi.Ema(preset?.Ema ?? 10);
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
			decimal pl = 1.01m; // Lowest Percentile - 1.10=90%, 1.05=95%, 1.01=99%
			int ltLb = 40; // Long-Term Look Back Current Bar Has To Close Below This Value OR Medium Term")
			int mtLb = 14; // Medium-Term Look Back Current Bar Has To Close Below This Value OR Long Term")
			int str = 3; // Entry Price Action Strength--Close > X Bars Back")

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
				if (i < ltLb)
				{
					result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
				}
				else
				{
					//Filtered Bar Criteria
					bool upRange = low[i] > low[i - 1] && close[i] > high[i - 1];
					bool upRangeAggr = close[i] > close[i - 1] && close[i] > open[i - 1];

					//Filtered Criteria
					bool filtered = (wvfs[i - 1] >= upperRanges[i - 1] || wvfs[i - 1] >= rangeHighs[i - 1]) && wvfs[i] < upperRanges[i] && wvfs[i] < rangeHighs[i];
					bool filteredAggr = (wvfs[i - 1] >= upperRanges[i - 1] || wvfs[i - 1] >= rangeHighs[i - 1]) && !(wvfs[i] < upperRanges[i] && wvfs[i] < rangeHighs[i]);

					bool filteredAlert = upRange && close[i] > close[i - str] && (close[i] < close[i - ltLb] || close[i] < close[i - mtLb]) && filtered;
					bool aggressiveAlert = upRangeAggr && close[i] > close[i - str] && (close[i] < close[i - ltLb] || close[i] < close[i - mtLb]) && filteredAggr;

					if ((filteredAlert || aggressiveAlert) && rsi[i] > ema[i] && rsi[i - 1] < ema[i - 1])
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.BUY));
					}
					else if (stochRsi.K[i] > 80 && stochRsi.K[i] > stochRsi.D[i] && stochRsi.K[i - 1] < stochRsi.D[i - 1])
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.SELL));
					}
					else
					{
						result.Add((candles.ElementAt(i), TradingAdviceCode.HOLD));
					}
				}
			}

			return result;
		}

		private decimal GetStandardDeviation (List<decimal> decimalList)
		{
			decimal average = decimalList.Average();
			decimal sumOfDerivation = 0;
			foreach (decimal value in decimalList)
			{
				sumOfDerivation += value * value;
			}
			decimal sumOfDerivationAverage = sumOfDerivation / (decimalList.Count - 1);
			return (decimal)Math.Sqrt((double)(sumOfDerivationAverage - average * average));
		}
	}
}
