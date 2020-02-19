using System.Collections.Generic;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
{
    public static class RelativeVigorIndexExtension
    {
        public static RelativeVigorIndexResult Rvi (this IEnumerable<ICandle> source)
        {
            RelativeVigorIndexIndicator rvi = new RelativeVigorIndexIndicator();
			
            return rvi.Get(source.ToArray(), new EmptyOption());
        }
    }
}