using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using core.Trading.TAIndicators;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class WmaExtension
    {
        public static SeriesIndicatorResult Wma (this IEnumerable<ICandle> source, int period, CandleVariables type)
        {
            WmaIndicator wma = new WmaIndicator();
            WmaOptions options = new WmaOptions() { Period = period, CandleVariable = type};
			
            return wma.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Wma (this IEnumerable<decimal> source, int period)
        {
            WmaIndicator wma = new WmaIndicator();
            WmaOptions options = new WmaOptions() { Period = period, CandleVariable = null};
			
            return wma.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Wma (this IEnumerable<decimal?> source, int period)
        {
            WmaIndicator wma = new WmaIndicator();
            WmaOptions options = new WmaOptions() { Period = period, CandleVariable = null};
			
            return wma.Get(source.ToArray(), options);
        }
    }
}