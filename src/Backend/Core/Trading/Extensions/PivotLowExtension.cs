using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class PivotLowExtension
	{
		public static List<decimal?> PivotLow (this IEnumerable<ICandle> candles, int? barsLeft, int? barsRight, bool? fillNullValues)
		{
			barsLeft ??= 4;
			barsRight ??= 2;
			fillNullValues ??= false;

			IIndicatorOptions options = new PivotLowOptions(barsLeft.Value, barsRight.Value, fillNullValues.Value);
			PivotLow pivotLow = new PivotLow();
			return pivotLow.Get(candles, options);
		}
	}
}
