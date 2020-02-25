using System.Collections.Generic;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;

namespace TechanCore.Indicators.Extensions
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