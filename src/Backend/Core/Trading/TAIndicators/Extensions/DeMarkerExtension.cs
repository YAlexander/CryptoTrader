using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class DeMarkerExtension
    {
        public static SeriesIndicatorResult DeMarker (this IEnumerable<ICandle> source, int period)
        {
            DeMarkerIndicator deMarker = new DeMarkerIndicator();
            DeMarkerOptions options = new DeMarkerOptions { Period = period };
			
            return deMarker.Get(source.ToArray(), options);
        }
    }
}