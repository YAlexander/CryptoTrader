using System.Collections.Generic;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class CmoExtension
    {
        public static SeriesIndicatorResult Cmo (this IEnumerable<ICandle> source, int period)
        {
            ChandeMomentumOscillatorIndicator cmo = new ChandeMomentumOscillatorIndicator();
            CmoOptions options = new CmoOptions { Period = period };
			
            return cmo.Get(source.ToArray(), options);
        }
    }
}