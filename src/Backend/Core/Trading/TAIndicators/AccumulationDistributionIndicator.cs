using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class AccumulationDistributionIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Accumulation/Distribution (AD) Indicator";

        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            decimal?[] result = new decimal?[source.Length];
            result[0] = ((source[0].Close - source[0].Low) - (source[0].High - source[0].Close)) /
                (source[0].High - source[0].Low) * source[0].Volume;

            for (int i = 1; i < source.Length; i++)
            {
                result[i] = ((source[i].Close - source[i].Low) - (source[i].High - source[i].Close)) /
                    (source[i].High - source[i].Low) * source[i].Volume + result[i - 1];
            }
            
            return new SeriesIndicatorResult() { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, EmptyOption options)
        {
            throw new System.NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, EmptyOption options)
        {
            throw new System.NotImplementedException();
        }
    }
}