using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
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