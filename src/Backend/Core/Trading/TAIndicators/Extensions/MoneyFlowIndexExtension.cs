using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class MoneyFlowIndexExtension
    {
        public static SeriesIndicatorResult Mfi (this IEnumerable<ICandle> source, int period)
        {
            MfiIndicator mfi = new MfiIndicator();
            MfiOptions options = new MfiOptions { Period = period };
			
            return mfi.Get(source.ToArray(), options);
        }
    }
}