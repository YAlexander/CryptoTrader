using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
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