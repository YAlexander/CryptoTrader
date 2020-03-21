using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
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