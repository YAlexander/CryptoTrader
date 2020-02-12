using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;
using Core.Trading.Models;

namespace Core.Trading.Extensions
{
	public static class MamaExtension
	{
		public static MamaItem Mama (this IEnumerable<ICandle> candles, double? fastLimit, double? slowLimit)
		{
			fastLimit ??= 0;
			slowLimit ??= 0;

			IIndicatorOptions options = new MamaOptions(fastLimit.Value, slowLimit.Value);
			Mama mama = new Mama();
			return (MamaItem)mama.Get(candles, options);
		}
	}
}
