using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class RsiExtension
	{
		public static SeriesIndicatorResult Rsi (this IEnumerable<ICandle> source, int period)
		{
			RsiIndicator rsi = new RsiIndicator();
			RsiOptions options = new RsiOptions { Period = period };
			
			return rsi.Get(source.ToArray(), options);
		}
	}
}