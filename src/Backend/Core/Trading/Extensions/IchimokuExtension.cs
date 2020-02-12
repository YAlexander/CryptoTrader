using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;
using Core.Trading.Models;

namespace Core.Trading.Extensions
{
	public static class IchimokuExtension
	{
		public static IchimokuItem Ichimoku (this IEnumerable<ICandle> candles, int? conversionLinePeriod = null, int? baseLinePeriod = null, int? laggingSpanPeriods = null, int? displacement = null)
		{
			conversionLinePeriod ??= 20;
			baseLinePeriod ??= 60;
			laggingSpanPeriods ??= 120;
			displacement ??= 30;

			IIndicatorOptions options = new IchimokuOptions(conversionLinePeriod.Value, baseLinePeriod.Value, laggingSpanPeriods.Value, displacement.Value);
			Ichimoku ichimoku = new Ichimoku();
			return (IchimokuItem)ichimoku.Get(candles, options);
		}
	}
}