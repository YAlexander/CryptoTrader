using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class MfiExtension
	{
		public static List<decimal?> Mfi (this IEnumerable<ICandle> candles, int? period = null)
		{
			period ??= 14;

			IIndicatorOptions options = new MfiOptions(period.Value);
			Mfi mfi = new Mfi();
			return mfi.Get(candles, options);
		}
	}
}
