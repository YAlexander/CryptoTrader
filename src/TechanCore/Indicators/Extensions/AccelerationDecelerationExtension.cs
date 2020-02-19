using System.Collections.Generic;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators.Extensions
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