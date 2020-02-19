using System.Collections.Generic;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
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