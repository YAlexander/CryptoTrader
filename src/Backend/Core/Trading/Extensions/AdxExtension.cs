using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class AdxExtension
	{
		public static List<decimal?> Adx (this IEnumerable<ICandle> candles, int period)
		{
			Adx sma = new Adx();
			return sma.Get(candles, options);
		}

		public static List<decimal?> Adx (this IEnumerable<decimal?> candles, int period)
		{
			IIndicatorOptions options = new AdxOptions(period);
			Adx sma = new Adx();
			return sma.Get(candles, options);
		}
	}
}
