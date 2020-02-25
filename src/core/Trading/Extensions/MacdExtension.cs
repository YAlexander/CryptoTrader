using core.Abstractions;
using core.Trading.Indicators;
using core.Trading.Models;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class MacdExtension
	{
		public static MacdItem Macd (this IEnumerable<ICandle> candles, int? fastPeriod = null, int? slowPeriod = null, int? signalPeriod = null)
		{
			fastPeriod ??= 12;
			slowPeriod ??= 26;
			signalPeriod ??= 9;

			IIndicatorOptions options = new MacdOptions(fastPeriod.Value, slowPeriod.Value, signalPeriod.Value);
			Macd macd = new Macd();
			return (MacdItem)macd.Get(candles, options);
		}
	}
}
