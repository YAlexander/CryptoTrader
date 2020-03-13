using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Persistence.Entities;

namespace Core.Helpers
{
	public static class CandleHelpers
	{
		public static ICandle[] GroupCandles(this IEnumerable<ICandle> candles, Timeframes timeFrame)
		{
			TimeSpan interval = timeFrame switch
			{
				Timeframes.FIVE_MINUTES => new TimeSpan(0, 0, 5, 0),
				Timeframes.QUARTER_HOUR => new TimeSpan(0, 0, 15, 0),
				Timeframes.HALF_HOUR => new TimeSpan(0, 0, 30, 0),
				Timeframes.HOUR => new TimeSpan(0, 1, 0, 0),
				Timeframes.FOUR_HOUR => new TimeSpan(0, 4, 0, 0),
				Timeframes.DAY => new TimeSpan(1, 0, 0, 0),
				Timeframes.WEEK => new TimeSpan(7, 0, 0, 0),
				_ => new TimeSpan(0, 0, 1, 0)
			};

			return candles.GroupBy(s => s.Time.Ticks / interval.Ticks)
				.Select(g =>
				{
					return (ICandle) new Candle()
					{
						Time = new DateTime(g.Key * interval.Ticks),
						Open = g.First().Open,
						Close = g.Last().Close,
						High = g.Max(x => x.High),
						Low = g.Min(x => x.Low),
						Volume = g.Sum(x => x.Volume)
					};
				})
				.OrderBy(x => x.Time)
				.ToArray();
		}
	}
}