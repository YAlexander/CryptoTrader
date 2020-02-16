using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using core.Trading.TAIndicators;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class StandardDeviationExtension
    {
        public static SeriesIndicatorResult StDev (this IEnumerable<ICandle> source, int period, CandleVariables type)
        {
            StandardDeviationIndicator stDev = new StandardDeviationIndicator();
            StandardDeviationOptions options = new StandardDeviationOptions() { Period = period, CandleVariable = type};
			
            return stDev.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult StDev (this IEnumerable<decimal> source, int period)
        {
            StandardDeviationIndicator stDev = new StandardDeviationIndicator();
            StandardDeviationOptions options = new StandardDeviationOptions() { Period = period, CandleVariable = null};
			
            return stDev.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult StDev (this IEnumerable<decimal?> source, int period)
        {
            StandardDeviationIndicator stDev = new StandardDeviationIndicator();
            StandardDeviationOptions options = new StandardDeviationOptions() { Period = period, CandleVariable = null};
			
            return stDev.Get(source.ToArray(), options);
        }
    }
}