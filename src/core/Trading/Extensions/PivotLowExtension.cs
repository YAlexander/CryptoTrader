using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class PivotLowExtension
	{
		public static List<decimal?> PivotLow (this IEnumerable<ICandle> candles, int? barsLeft, int? barsRight, bool? fillNullValues)
		{
			barsLeft = barsLeft ?? 4;
			barsRight = barsRight ?? 2;
			fillNullValues = fillNullValues ?? false;

			IIndicatorOptions options = new PivotLowOptions(barsLeft.Value, barsRight.Value, fillNullValues.Value);
			PivotLow pivotLow = new PivotLow();
			return pivotLow.Get(candles, options);
		}
	}
}
