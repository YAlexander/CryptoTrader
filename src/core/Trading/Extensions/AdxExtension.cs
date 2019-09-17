using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class AdxExtension
	{
		public static List<decimal?> Adx (this IEnumerable<ICandle> candles, int? period = null)
		{
			period ??= 14;

			IIndicatorOptions options = new AdxOptions(period.Value);
			Adx sma = new Adx();
			return sma.Get(candles, options);
		}

		public static List<decimal?> Adx (this IEnumerable<decimal?> candles, int? period = null)
		{
			period ??= 14;

			IIndicatorOptions options = new AdxOptions(period.Value);
			Adx sma = new Adx();
			return sma.Get(candles, options);
		}
	}
}
