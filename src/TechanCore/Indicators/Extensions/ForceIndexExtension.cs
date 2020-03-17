using System.Collections.Generic;
using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class ForceIndexExtension
    {
        public static SeriesIndicatorResult ForceIndex (this IEnumerable<ICandle> source, int period)
        {
            ForceIndexIndicator fi = new ForceIndexIndicator();
            ForceIndexOptions options = new ForceIndexOptions { Period = period };
			
            return fi.Get(source.ToArray(), options);
        }
    }
}