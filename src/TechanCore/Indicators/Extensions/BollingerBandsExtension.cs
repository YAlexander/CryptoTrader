using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
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