using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class MinusDiExtension
	{
		public static List<decimal?> MinusDi (this IEnumerable<ICandle> candles, int? period)
		{
			period ??= 14;

			IIndicatorOptions options = new MinusDiOptions(period.Value);
			MinusDi minusDi = new MinusDi();
			return minusDi.Get(candles, options);
		}
	}
}
