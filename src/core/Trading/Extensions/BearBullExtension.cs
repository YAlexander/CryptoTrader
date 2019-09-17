using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class BearBullExtension
	{
		public static List<decimal?> BearBull (this IEnumerable<ICandle> candles)
		{
			BearBull bearBull = new BearBull();
			return bearBull.Get(candles);
		}
	}
}
