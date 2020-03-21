using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class StochRsiExtension
	{
		public static SeriesIndicatorResult StocRsi(this IEnumerable<ICandle> source, int rsiPeriod)
		{
			StochRsiIndicator stocRsi = new StochRsiIndicator();
			StochRsiOptions options = new StochRsiOptions { RsiPeriod = rsiPeriod };
			return stocRsi.Get(source.ToArray(), options);
		}
	}
}