using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class PriceVolumeTrendExtension
    {
        public static SeriesIndicatorResult PriceVolumeTrend (this IEnumerable<ICandle> source)
        {
            PriceVolumeTrendIndicator pvt = new PriceVolumeTrendIndicator();
			
            return pvt.Get(source.ToArray(), new EmptyOption());
        }
    }
}