using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class WmaExtension
    {
        public static SeriesIndicatorResult Wma (this IEnumerable<ICandle> source, int period, CandleVariables type)
        {
            WmaIndicator wma = new WmaIndicator();
            WmaOptions options = new WmaOptions { Period = period, CandleVariable = type};
			
            return wma.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Wma (this IEnumerable<decimal> source, int period)
        {
            WmaIndicator wma = new WmaIndicator();
            WmaOptions options = new WmaOptions { Period = period, CandleVariable = null};
			
            return wma.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Wma (this IEnumerable<decimal?> source, int period)
        {
            WmaIndicator wma = new WmaIndicator();
            WmaOptions options = new WmaOptions { Period = period, CandleVariable = null};
			
            return wma.Get(source.ToArray(), options);
        }
    }
}