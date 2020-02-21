using System.Collections.Generic;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class DerivativeOscillatorExtension
	{
		public static SeriesIndicatorResult DerivativeOscillator (this IEnumerable<ICandle> source, int rsiPeriod, int emaFastPeriod, int emaSlowPeriod, int smaPeriod)
		{
			DerivativeOscillatorIndicator deMarker = new DerivativeOscillatorIndicator();
			DerivativeOscillatorOptions options = new DerivativeOscillatorOptions
			{
				RsiPeriod = rsiPeriod,
				EmaFastPeriod = emaFastPeriod,
				EmaSlowPeriod = emaFastPeriod,
				SmaPeriod = smaPeriod
			};
			
			return deMarker.Get(source.ToArray(), options);
		}
	}
}