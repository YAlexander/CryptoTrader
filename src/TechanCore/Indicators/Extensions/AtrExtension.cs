using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class AtrExtension
	{
		public static SeriesIndicatorResult Atr (this IEnumerable<ICandle> source, int period)
		{
			AtrIndicator atr = new AtrIndicator();
			AtrOptions options = new AtrOptions {Period = period};
			
			return atr.Get(source.ToArray(), options);
		}
	}
}