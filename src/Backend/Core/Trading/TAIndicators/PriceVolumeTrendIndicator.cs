using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class PriceVolumeTrendIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Price Volume Trend (PVT) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            decimal?[] result = new decimal?[source.Length];
            
            for (int i = 1; i < source.Length; i++)
            {
                if (i > 1)
                {
                    result[i] = ((source[i].Close - source[i - 1].Close) / source[i - 1].Close) * source[i].Volume +  result[i - 1];
                }
                else
                {
                    result[i] = ((source[i].Close - source[i - 1].Close) / source[i - 1].Close) * source[i].Volume;
                }
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