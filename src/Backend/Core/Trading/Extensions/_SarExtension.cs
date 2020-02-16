using Core.Trading.Indicators.Options;
using System.Collections.Generic;
using Contracts;
using Contracts.Trading;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
{
	public static class SarExtension
	{
		public static List<decimal?> Sar (this IEnumerable<ICandle> candles, double? accelerationFactor = null, double? maximumAccelerationFactor = null)
		{
			accelerationFactor ??= 0.02;
			maximumAccelerationFactor ??= 0.2;

			IIndicatorOptions options = new SarOptions(accelerationFactor.Value, maximumAccelerationFactor.Value);
			Sar sar = new Sar();
			return sar.Get(candles, options);
		}
	}
}