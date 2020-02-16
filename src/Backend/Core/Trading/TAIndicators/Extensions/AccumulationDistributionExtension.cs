using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
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