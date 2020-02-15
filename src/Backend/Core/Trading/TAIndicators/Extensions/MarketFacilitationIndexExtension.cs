using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
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