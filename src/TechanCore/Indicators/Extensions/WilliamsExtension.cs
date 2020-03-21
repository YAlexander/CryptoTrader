using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class WilliamsExtension
    {
        public static SeriesIndicatorResult Williams (this IEnumerable<ICandle> source, int period)
        {
            WilliamsPercentRangeIndicator wpr = new WilliamsPercentRangeIndicator();
            WilliamsPercentRangeOptions options = new WilliamsPercentRangeOptions { Period = period };
			
            return wpr.Get(source.ToArray(), options);
        }
    }
}