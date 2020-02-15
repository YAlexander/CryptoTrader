using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class VolumeExtension
    {
        public static SeriesIndicatorResult Volume (this IEnumerable<ICandle> source)
        {
            VolumeIndicator vol = new VolumeIndicator();
			
            return vol.Get(source.ToArray(), new EmptyOption());
        }
    }
}