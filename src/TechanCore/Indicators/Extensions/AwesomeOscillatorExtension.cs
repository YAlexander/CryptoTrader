using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class AwesomeOscillatorExtension
	{
		public static SeriesIndicatorResult AwesomeOscillator (this IEnumerable<ICandle> candles, int fastPeriod, int slowPeriod)
		{
			AwesomeOscillatorIndicator awesome = new AwesomeOscillatorIndicator();
			AwesomeOscillatorOptions options = new AwesomeOscillatorOptions { FastSmaPeriod = fastPeriod, SlowSmaPeriod = slowPeriod };
			return awesome.Get(candles.ToArray(), options);
		}
	}
}