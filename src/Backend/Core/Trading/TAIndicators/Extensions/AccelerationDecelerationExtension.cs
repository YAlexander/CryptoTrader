using System.Collections.Generic;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators.Extensions
{
    public static class AccelerationDecelerationExtension
    {
        public static SeriesIndicatorResult AccelerationDeceleration (this IEnumerable<ICandle> source)
        {
            AccelerationDecelerationIndicator accDec = new AccelerationDecelerationIndicator();
            return accDec.Get(source.ToArray(), new EmptyOption());
        }
    }
}