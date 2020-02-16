using System.Collections.Generic;
using Contracts;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
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
