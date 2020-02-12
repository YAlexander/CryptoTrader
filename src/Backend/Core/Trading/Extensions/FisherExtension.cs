using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class FisherExtension
	{
		public static List<decimal?> Fisher (this IEnumerable<ICandle> candles, int? period = null)
		{
			period ??= 10;

			IIndicatorOptions options = new AdxOptions(period.Value);
			Fisher sma = new Fisher();
			return sma.Get(candles, options);
		}

		public static List<decimal?> Fisher (this IEnumerable<decimal?> candles, int? period)
		{
			period ??= 10;

			IIndicatorOptions options = new AdxOptions(period.Value);
			Fisher sma = new Fisher();
			return sma.Get(candles, options);
		}
	}
}
