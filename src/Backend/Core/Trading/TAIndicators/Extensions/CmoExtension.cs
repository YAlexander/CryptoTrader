using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
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