using System;
using Contracts;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class DeMarkerIndicator : BaseIndicator<DeMarkerOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "DeMarker’s (DeMarker) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, DeMarkerOptions options)
        {
            decimal?[] result = new decimal?[source.Length];
            
            decimal?[] deMin = new decimal?[source.Length];
            decimal?[] deMax = new decimal?[source.Length];

            for (int i = 1; i < source.Length; i++)
            {
                deMax[i] = source[i].High > source[i - 1].High ? source[i].High - source[i - 1].High : 0;
                deMin[i] = source[i].Low < source[i - 1].Low ? source[i - 1].Low - source[i].Low : 0;
            }

            decimal?[] smaMax = deMax.Sma(options.Period).Result;
            decimal?[] smaMin = deMin.Sma(options.Period).Result;

            for (int i = 0; i < source.Length; i++)
            {
                if (smaMax[i].HasValue && smaMin[i].HasValue)
                {
                    result[i] = smaMax[i] / (smaMax[i] + smaMin[i]);
                }
                else
                {
                    result[i] = null;
                }
            }

            return new SeriesIndicatorResult { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, DeMarkerOptions options)
        {
            throw new NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, DeMarkerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}