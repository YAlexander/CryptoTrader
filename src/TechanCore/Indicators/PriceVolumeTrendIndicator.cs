using System;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class PriceVolumeTrendIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Price Volume Trend (PVT) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            decimal?[] result = new decimal?[source.Length];
            result[1] = (source[1].Close - source[0].Close) / source[0].Close * source[1].Volume;
            
            for (int i = 2; i < source.Length; i++)
            { 
                result[i] = ((source[i].Close - source[i - 1].Close) / source[i - 1].Close) * source[i].Volume +  result[i - 1];
            }

            return new SeriesIndicatorResult { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, EmptyOption options)
        {
            throw new NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, EmptyOption options)
        {
            throw new NotImplementedException();
        }
    }
}