using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class MacdExtension
	{
		public static MacdIndicatorResult Macd (this IEnumerable<ICandle> source, int fastPeriod, int slowPeriod, int signalPeriod)
		{
			MacdIndicator macd = new MacdIndicator();
			MacdOptions options = new MacdOptions { FastPeriod = fastPeriod, SlowPeriod = slowPeriod, SignalPeriod = signalPeriod};
			return macd.Get(source.ToArray(), options);
		}
		
		public static MacdIndicatorResult Macd (this IEnumerable<decimal> source, int fastPeriod, int slowPeriod, int signalPeriod)
		{
			MacdIndicator macd = new MacdIndicator();
			MacdOptions options = new MacdOptions { FastPeriod = fastPeriod, SlowPeriod = slowPeriod, SignalPeriod = signalPeriod};
			return macd.Get(source.ToArray(), options);
		}
		
		public static MacdIndicatorResult Macd (this IEnumerable<decimal?> source, int fastPeriod, int slowPeriod, int signalPeriod)
		{
			MacdIndicator macd = new MacdIndicator();
			MacdOptions options = new MacdOptions { FastPeriod = fastPeriod, SlowPeriod = slowPeriod, SignalPeriod = signalPeriod};
			return macd.Get(source.ToArray(), options);
		}
	}
}