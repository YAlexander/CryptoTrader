using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class CciExtension
    {
        public static SeriesIndicatorResult Cci (this IEnumerable<ICandle> source, int period)
        {
            CciIndicator cci = new CciIndicator();
            CciOptions options = new CciOptions() { Period = period};
			
            return cci.Get(source.ToArray(), options);
        }
    }
}