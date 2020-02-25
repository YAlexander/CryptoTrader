using System;
using System.Collections.Generic;
using core.Abstractions;
using core.Infrastructure.Database.Entities;
using core.Trading.Indicators;

namespace Core.Trading.Indicators
{
	public class HeikinAshi : BaseIndicator
	{
		public override string Name { get; } = "Heikin Ashi";

		public override dynamic Get (IEnumerable<ICandle> source, IIndicatorOptions options = null)
		{
			List<ICandle> result = new List<ICandle>();
			ICandle previous = null;

			foreach (ICandle item in source)
			{
				ICandle candle = new Candle();
				candle.Id = item.Id;
				candle.Time = item.Time;
				candle.Volume = item.Volume;

				if (previous != null)
				{
					candle.Close = (item.High + item.Low + item.Close + item.Open) / 4;
					candle.Open = (previous.Open + previous.Close) / 2;
					candle.Low = Math.Min(Math.Min(item.Low, item.Open), item.Close);
					candle.High = Math.Max(Math.Max(item.High, item.Open), item.Close);
				}
				else
				{
					candle.Close = (item.High + item.Low + item.Close + item.Open) / 4;
					candle.Open = (item.Open + item.Close) / 2;
					candle.Low = Math.Min(Math.Min(item.Low, item.Open), item.Close);
					candle.High = Math.Max(Math.Max(item.High, item.Open), item.Close);
				}

				result.Add(candle);
				previous = item;
			}

			return result;
		}

		public override dynamic Get (IEnumerable<decimal?> candles, IIndicatorOptions options = null)
		{
			throw new NotImplementedException();
		}
	}
}
