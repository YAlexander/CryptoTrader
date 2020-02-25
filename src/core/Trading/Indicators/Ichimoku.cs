using System;
using System.Collections.Generic;
using System.Linq;
using core.Abstractions;
using core.Trading.Models;
using Core.Trading.Indicators.Options;

namespace core.Trading.Indicators
{
	public class Ichimoku : BaseIndicator
	{
		public override string Name => nameof(Ichimoku);

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			try
			{
				IchimokuOptions config = options != null ? (IchimokuOptions)options.Options : new IchimokuOptions(20, 60, 120, 30);

				List<decimal> highs = source.Select(x => x.High).ToList();
				List<decimal> lows = source.Select(x => x.Low).ToList();
				List<decimal> closes = source.Select(x => x.Close).ToList();

				var ichi = new IchimokuItem
				{
					TenkanSen = Donchian(source, config.ConversionLinePeriod, highs, lows),
					KijunSen = Donchian(source, config.BaseLinePeriod, highs, lows),
					SenkouSpanB = Donchian(source, config.LaggingSpanPeriods, highs, lows),
					SenkouSpanA = new List<decimal?>()
				};


				// SenkouSpanA is calculated...
				for (int i = 0; i < ichi.TenkanSen.Count; i++)
				{
					if (ichi.TenkanSen[i].HasValue && ichi.KijunSen[i].HasValue)
					{
						ichi.SenkouSpanA.Add((ichi.TenkanSen[i].Value + ichi.KijunSen[i].Value) / 2);
					}
					else
					{
						ichi.SenkouSpanA.Add(null);
					}
				}

				// Add the displacement for the cloud
				for (int i = 0; i < config.Displacement; i++)
				{
					ichi.SenkouSpanA.Insert(0, null);
					ichi.SenkouSpanB.Insert(0, null);
				}

				// Add the ChikouSpan
				ichi.ChikouSpan = new List<decimal?>();

				// Add the displacement for the lagging span
				var displacedCloses = closes.Skip(config.Displacement).ToList();

				for (int i = 0; i < closes.Count; i++)
				{
					if (i < closes.Count - config.Displacement)
					{
						ichi.ChikouSpan.Add(displacedCloses[i]);
					}
					else
					{
						ichi.ChikouSpan.Add(null);
					}
				}

				return ichi;
			}
			catch (Exception ex)
			{
				throw new Exception("Could not calculate ichimoku cloud");
			}
		}

		private static List<decimal?> Donchian (IEnumerable<ICandle> source, int period, List<decimal> highs, List<decimal> lows)
		{
			// Calculate the Tenkan-sen
			var result = new List<decimal?>();

			for (var i = 0; i < source.Count(); i++)
			{
				if (i < period)
				{
					// Get the highest high & lowest low of the last X items (X = conversionLinePeriod)
					var highestHigh = highs.GetRange(0, i + 1).Max();
					var lowestLow = lows.GetRange(0, i + 1).Min();

					result.Add((highestHigh + lowestLow) / 2);
				}
				else
				{
					// Get the highest high & lowest low of the last X items (X = conversionLinePeriod)
					var highestHigh = highs.GetRange(i - period, period).Max();
					var lowestLow = lows.GetRange(i - period, period).Min();

					result.Add((highestHigh + lowestLow) / 2);
				}
			}

			return result;
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}

