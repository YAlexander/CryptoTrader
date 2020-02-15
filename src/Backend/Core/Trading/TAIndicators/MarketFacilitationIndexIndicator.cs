using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class MarketFacilitationIndexIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Market facilitation Index (BW MFI) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            decimal?[] result = new decimal?[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[i] = (source[i].High - source[i].Low) / source[i].Volume;
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