using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class AtrTrailingStopExtension
    {
        public static decimal? AtrTrailingStop (this IEnumerable<ICandle> source, int period)
        {
            AtrTrailingStopIndicator ats = new AtrTrailingStopIndicator();
            AtrTrailingStopOptions options = new AtrTrailingStopOptions { Period = period };
			
            return ats.Get(source.ToArray(), options).Result.LastOrDefault();
        }
    }
}