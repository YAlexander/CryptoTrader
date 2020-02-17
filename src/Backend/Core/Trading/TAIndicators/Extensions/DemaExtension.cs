using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class DemaExtension
    {
        public static SeriesIndicatorResult Dema (this IEnumerable<ICandle> source, int period)
        {
            DoubleExponentialMovingAverageIndicator dema = new DoubleExponentialMovingAverageIndicator();
            DemaOptions options = new DemaOptions { Period = period };
			
            return dema.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Dema (this IEnumerable<decimal> source, int period)
        {
            DoubleExponentialMovingAverageIndicator dema = new DoubleExponentialMovingAverageIndicator();
            DemaOptions options = new DemaOptions { Period = period };
			
            return dema.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Dema (this IEnumerable<decimal?> source, int period)
        {
            DoubleExponentialMovingAverageIndicator dema = new DoubleExponentialMovingAverageIndicator();
            DemaOptions options = new DemaOptions { Period = period };
			
            return dema.Get(source.ToArray(), options);
        }
    }
}