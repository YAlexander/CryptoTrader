using core.Abstractions;
using core.Trading.Indicators.Options;
using core.TypeCodes;
using Core.Trading.Indicators;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class EmaExtension
	{
		public static List<decimal?> Ema (this IEnumerable<ICandle> candles, int period)
		{
			IIndicatorOptions options = new EmaOptions(period, CandleVariableCode.CLOSE);
			Ema ema = new Ema();
			return ema.Get(candles, options);
		}

		public static List<decimal?> Ema (this IEnumerable<decimal?> candles, int period)
		{
			IIndicatorOptions options = new EmaOptions(period, CandleVariableCode.CLOSE);
			Ema ema = new Ema();
			return ema.Get(candles, options);
		}
	}
}
