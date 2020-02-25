using core.Abstractions;
using core.Trading.Indicators;
using core.Trading.Models;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class IchimokuExtension
	{
		public static IchimokuItem Ichimoku (this IEnumerable<ICandle> candles, int? conversionLinePeriod = null, int? baseLinePeriod = null, int? laggingSpanPeriods = null, int? displacement = null)
		{
			conversionLinePeriod = conversionLinePeriod ?? 20;
			baseLinePeriod = baseLinePeriod ?? 60;
			laggingSpanPeriods = laggingSpanPeriods ?? 120;
			displacement = displacement ?? 30;

			IIndicatorOptions options = new IchimokuOptions(conversionLinePeriod.Value, baseLinePeriod.Value, laggingSpanPeriods.Value, displacement.Value);
			Ichimoku ichimoku = new Ichimoku();
			return (IchimokuItem)ichimoku.Get(candles, options);
		}
	}
}