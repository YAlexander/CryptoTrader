using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class StandardDeviationExtension
    {
        public static SeriesIndicatorResult StDev (this IEnumerable<ICandle> source, int period, CandleVariables type)
        {
            StandardDeviationIndicator stDev = new StandardDeviationIndicator();
            StandardDeviationOptions options = new StandardDeviationOptions { Period = period, CandleVariable = type};
			
            return stDev.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult StDev (this IEnumerable<decimal> source, int period)
        {
            StandardDeviationIndicator stDev = new StandardDeviationIndicator();
            StandardDeviationOptions options = new StandardDeviationOptions { Period = period, CandleVariable = null};
			
            return stDev.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult StDev (this IEnumerable<decimal?> source, int period)
        {
            StandardDeviationIndicator stDev = new StandardDeviationIndicator();
            StandardDeviationOptions options = new StandardDeviationOptions { Period = period, CandleVariable = null};
			
            return stDev.Get(source.ToArray(), options);
        }
    }
}