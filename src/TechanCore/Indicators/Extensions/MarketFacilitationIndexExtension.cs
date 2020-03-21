using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class MarketFacilitationIndexExtension
    {
        public static SeriesIndicatorResult BwMfi (this IEnumerable<ICandle> source)
        {
            MarketFacilitationIndexIndicator bwMfi = new MarketFacilitationIndexIndicator();
			
            return bwMfi.Get(source.ToArray(), new EmptyOption());
        }
    }
}