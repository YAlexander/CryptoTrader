using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Indicators;
using core.Trading.Indicators.Options;
using core.TypeCodes;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class WmaExtension
	{
		public static List<decimal?> Wma (this IEnumerable<ICandle> candles, int? period = null, ICandleVariableCode type = null)
		{
			period ??= 14;
			type ??= CandleVariableCode.CLOSE;

			IIndicatorOptions options = new WmaOptions(period.Value, type);
			Wma wma = new Wma();
			return wma.Get(candles, options);
		}

		public static List<decimal?> Wma (this IEnumerable<decimal?> candles, int? period = null, ICandleVariableCode type = null)
		{
			period ??= 14;
			type ??= CandleVariableCode.CLOSE;

			IIndicatorOptions options = new WmaOptions(period.Value, type);
			Wma wma = new Wma();
			return wma.Get(candles, options);
		}
	}
}
