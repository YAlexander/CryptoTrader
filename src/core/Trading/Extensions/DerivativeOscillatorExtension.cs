using core.Abstractions;
using core.Trading.Indicators;
using Core.Trading.Indicators.Options;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class DerivativeOscillatorExtension
	{
		public static List<decimal?> DerivativeOscillator (this IEnumerable<ICandle> candles)
		{
			DerivativeOscillator oscillator = new DerivativeOscillator();
			return oscillator.Get(candles);
		}
	}
}
