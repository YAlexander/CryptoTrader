using core.Abstractions;
using core.Trading.Indicators;
using System.Collections.Generic;

namespace core.Trading.Extensions
{
	public static class AwesomeExtension
	{
		public static List<decimal?> AwesomeOscillator (this IEnumerable<ICandle> candles)
		{
			AwesomeOscillator awesome = new AwesomeOscillator();
			return awesome.Get(candles);
		}
	}
}
