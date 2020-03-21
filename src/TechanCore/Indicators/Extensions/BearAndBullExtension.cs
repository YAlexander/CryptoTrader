using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
	public static class BearAndBullExtension
	{
		public static SeriesIndicatorResult  BearAndBull (this IEnumerable<ICandle> source)
		{
			BearAndBullIndicator cci = new BearAndBullIndicator();
			return cci.Get(source.ToArray(), new EmptyOption());
		}
	}
}