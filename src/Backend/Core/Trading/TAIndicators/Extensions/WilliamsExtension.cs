using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class WilliamsExtension
    {
        public static SeriesIndicatorResult Williams (this IEnumerable<ICandle> source, int period)
        {
            WilliamsPercentRangeIndicator wpr = new WilliamsPercentRangeIndicator();
            WilliamsPercentRangeOptions options = new WilliamsPercentRangeOptions() { Period = period };
			
            return wpr.Get(source.ToArray(), options);
        }
    }
}