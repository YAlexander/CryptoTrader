using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class RsiExtension
	{
		public static List<decimal?> Rsi (this IEnumerable<ICandle> candles, int? period = null)
		{
			period ??= 14;

			IIndicatorOptions options = new RsiOptions(period.Value);
			Rsi rsi = new Rsi();
			return rsi.Get(candles, options);
		}

		public static List<decimal?> Rsi (this IEnumerable<decimal?> candles, int? period = null)
		{
			period ??= 14;

			IIndicatorOptions options = new RsiOptions(period.Value);
			Rsi rsi = new Rsi();
			return rsi.Get(candles, options);
		}
	}
}
