using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class PivotHighExtension
	{
		public static List<decimal?> PivotHigh (this IEnumerable<ICandle> candles, int? barsLeft, int? barsRight, bool? fillNullValues)
		{
			barsLeft ??= 4;
			barsRight ??= 2;
			fillNullValues ??= false;

			IIndicatorOptions options = new PivotHighOptions(barsLeft.Value, barsRight.Value, fillNullValues.Value);
			PivotHigh pivotHigh = new PivotHigh();
			return pivotHigh.Get(candles, options);
		}
	}
}
