using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class PlusDiExtension
	{
		public static List<decimal?> PlusDi (this IEnumerable<ICandle> candles, int? period)
		{
			period ??= 14;

			IIndicatorOptions options = new PlusDiOptions(period.Value);
			PlusDi plusDi = new PlusDi();
			return plusDi.Get(candles, options);
		}
	}
}
