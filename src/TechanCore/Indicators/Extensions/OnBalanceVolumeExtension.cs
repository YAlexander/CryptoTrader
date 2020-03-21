using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class OnBalanceVolumeExtension
    {
        public static SeriesIndicatorResult Obv (this IEnumerable<ICandle> source)
        {
            OnBalanceVolumeIndicator obv = new OnBalanceVolumeIndicator();
			
            return obv.Get(source.ToArray(), new EmptyOption());
        }
    }
}