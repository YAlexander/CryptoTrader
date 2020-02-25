using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class MomExtension
	{
		public static List<decimal?> Mom (this IEnumerable<ICandle> candles, int? period)
		{
			period = period ?? 10;

			IIndicatorOptions options = new MomOptions(period.Value);
			Mom mom = new Mom();
			return mom.Get(candles, options);
		}

		public static List<decimal?> Mom (this IEnumerable<decimal?> candles, int? period)
		{
			period = period ?? 10;

			IIndicatorOptions options = new MomOptions(period.Value);
			Mom mom = new Mom();
			return mom.Get(candles, options);
		}
	}
}
