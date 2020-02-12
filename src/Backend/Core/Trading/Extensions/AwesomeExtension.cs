using System.Collections.Generic;
using Contracts;
using Core.Trading.Indicators;

namespace Core.Trading.Extensions
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
