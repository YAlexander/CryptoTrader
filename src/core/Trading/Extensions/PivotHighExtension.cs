using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class PivotHighExtension
	{
		public static List<decimal?> PivotHigh (this IEnumerable<ICandle> candles, int? barsLeft, int? barsRight, bool? fillNullValues)
		{
			barsLeft = barsLeft ?? 4;
			barsRight = barsRight ?? 2;
			fillNullValues = fillNullValues ?? false;

			IIndicatorOptions options = new PivotHighOptions(barsLeft.Value, barsRight.Value, fillNullValues.Value);
			PivotHigh pivotHigh = new PivotHigh();
			return pivotHigh.Get(candles, options);
		}
	}
}
