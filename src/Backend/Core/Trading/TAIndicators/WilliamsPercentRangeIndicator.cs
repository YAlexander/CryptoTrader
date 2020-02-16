using System;
using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class WilliamsPercentRangeIndicator : BaseIndicator<WilliamsPercentRangeOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Williams Percent Range (William %R) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] candles, WilliamsPercentRangeOptions options)
        {
            Span<ICandle> source = new Span<ICandle>(candles);
            decimal?[] result = new decimal?[source.Length];
            
            for (int i = 0; i < source.Length; i++)
            {   
                if (i >= options.Period - 1)
                {
                    int startIndex = i - (options.Period - 1);
                    decimal highest = source.Slice(startIndex, i).ToArray().Max(x => x.High);
                    decimal lowest = source.Slice(startIndex, i).ToArray().Min(x => x.Low);

                    result[i] = (highest - source[i].Close) / (highest - lowest) * 100;;
                }
                else
                {
                    result[i] = null;
                }
            }

            return new SeriesIndicatorResult() { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, WilliamsPercentRangeOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, WilliamsPercentRangeOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}