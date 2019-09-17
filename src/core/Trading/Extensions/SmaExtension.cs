using core.Abstractions;
using core.Abstractions.TypeCodes;
using core.Trading.Indicators;
using core.Trading.Indicators.Options;
using core.TypeCodes;
using System.Collections.Generic;
using System.Linq;

namespace core.Trading.Extensions
{
	public static class SmaExtension
	{
		public static List<decimal?> Sma (this IEnumerable<ICandle> candles, int period, ICandleVariableCode variable = null)
		{
			variable = variable ?? CandleVariableCode.CLOSE;

			IIndicatorOptions options = new SmaOptions(period, variable);
			Sma sma = new Sma();
			return sma.Get(candles, options);
		}

		public static List<decimal?> Sma (this IEnumerable<decimal?> candles, int period, ICandleVariableCode variable = null)
		{
			variable = variable ?? CandleVariableCode.CLOSE;

			IIndicatorOptions options = new SmaOptions(period, variable);
			Sma sma = new Sma();
			return sma.Get(candles, options);
		}

		public static List<decimal?> Sma (this IEnumerable<decimal> candles, int period, ICandleVariableCode variable = null)
		{
			variable = variable ?? CandleVariableCode.CLOSE;

			IIndicatorOptions options = new SmaOptions(period, variable);
			IEnumerable<decimal?> values = candles.Select(x => (decimal?)x);
			Sma sma = new Sma();
			return sma.Get(values, options);
		}
	}
}
