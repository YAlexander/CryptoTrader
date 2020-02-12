using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class MomExtension
	{
		public static List<decimal?> Mom (this IEnumerable<ICandle> candles, int? period)
		{
			period ??= 10;

			IIndicatorOptions options = new MomOptions(period.Value);
			Mom mom = new Mom();
			return mom.Get(candles, options);
		}

		public static List<decimal?> Mom (this IEnumerable<decimal?> candles, int? period)
		{
			period ??= 10;

			IIndicatorOptions options = new MomOptions(period.Value);
			Mom mom = new Mom();
			return mom.Get(candles, options);
		}
	}
}
