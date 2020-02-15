using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
	public static class AwesomeOscillatorExtension
	{
		public static SeriesIndicatorResult AwesomeOscillator (this IEnumerable<ICandle> candles, int fastPeriod, int slowPeriod)
		{
			AwesomeOscillatorIndicator awesome = new AwesomeOscillatorIndicator();
			AwesomeOscillatorOptions options = new AwesomeOscillatorOptions() { FastSmaPeriod = fastPeriod, SlowSmaPeriod = slowPeriod };
			return awesome.Get(candles.ToArray(), options);
		}
	}
}