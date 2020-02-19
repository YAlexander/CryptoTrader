using System.Collections.Generic;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class MomentumExtension
    {
        public static SeriesIndicatorResult Momentum (this IEnumerable<ICandle> source, int period, CandleVariables type)
        {
            MomentumIndicator mom = new MomentumIndicator();
            MomentumOptions options = new MomentumOptions { Period = period };
			
            return mom.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Momentum (this IEnumerable<decimal> source, int period)
        {
            MomentumIndicator mom = new MomentumIndicator();
            MomentumOptions options = new MomentumOptions { Period = period };
			
            return mom.Get(source.ToArray(), options);
        }
		
        public static SeriesIndicatorResult Momentum (this IEnumerable<decimal?> source, int period)
        {
            MomentumIndicator mom = new MomentumIndicator();
            MomentumOptions options = new MomentumOptions { Period = period };
			
            return mom.Get(source.ToArray(), options);
        }
    }
}