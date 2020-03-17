using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
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