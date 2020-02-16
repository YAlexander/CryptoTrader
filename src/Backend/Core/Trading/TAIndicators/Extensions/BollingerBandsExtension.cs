using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class BollingerBandsExtension
    {
        public static BollingerBandsResult BollingerBands (this IEnumerable<ICandle> source, int period, double deviationUp, double deviationDown)
        {
            BollingerBandsIndicator bb = new BollingerBandsIndicator();
            BollingerBandsOptions options = new BollingerBandsOptions
            {
                Period = period,
                DeviationDown = deviationDown,
                DeviationUp = deviationUp
            };
			
            return bb.Get(source.ToArray(), options);
        }
    }
}