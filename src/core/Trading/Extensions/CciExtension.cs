using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class CciExtension
	{
		public static List<decimal?> Cci (this IEnumerable<ICandle> candles, int? period = null)
		{
			period ??= 14;

			IIndicatorOptions options = new CciOptions(period.Value);
			Cci cci = new Cci();
			return cci.Get(candles, options);
		}

		public static List<decimal?> Cci (this IEnumerable<decimal?> candles, int? period = null)
		{
			period ??= 14;

			IIndicatorOptions options = new CciOptions(period.Value);
			Cci cci = new Cci();
			return cci.Get(candles, options);
		}
	}
}
