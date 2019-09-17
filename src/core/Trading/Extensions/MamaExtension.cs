using core.Abstractions;
using core.Trading.Indicators;
using core.Trading.Models;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class MamaExtension
	{
		public static MamaItem Mama (this IEnumerable<ICandle> candles, double? fastLimit, double? slowLimit)
		{
			fastLimit = fastLimit ?? 0;
			slowLimit = slowLimit ?? 0;

			IIndicatorOptions options = new MamaOptions(fastLimit.Value, slowLimit.Value);
			Mama mama = new Mama();
			return (MamaItem)mama.Get(candles, options);
		}
	}
}
