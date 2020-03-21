using System.Collections.Generic;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class AccumulationDistributionExtension
    {
        public static SeriesIndicatorResult AccumulationDistribution (this IEnumerable<ICandle> source, int period, CandleVariables type)
        {
            AccumulationDistributionIndicator adi = new AccumulationDistributionIndicator();
			
            return adi.Get(source.ToArray(), new EmptyOption());
        }
    }
}