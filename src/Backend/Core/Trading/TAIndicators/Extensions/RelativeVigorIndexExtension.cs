using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
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