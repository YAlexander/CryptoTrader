using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class ForceIndexExtension
    {
        public static SeriesIndicatorResult ForceIndex (this IEnumerable<ICandle> source, int period)
        {
            ForceIndexIndicator fi = new ForceIndexIndicator();
            ForceIndexOptions options = new ForceIndexOptions() { Period = period };
			
            return fi.Get(source.ToArray(), options);
        }
    }
}