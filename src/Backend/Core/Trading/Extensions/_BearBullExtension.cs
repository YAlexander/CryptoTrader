using System.Collections.Generic;
using Contracts;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
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
