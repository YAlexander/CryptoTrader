using Contracts;
using Core.Trading.TAIndicators.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class ForceIndexIndicator : BaseIndicator<ForceIndexOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Force Index (FI) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, ForceIndexOptions options)
        {
            decimal?[] rawForce = new decimal?[source.Length];

            for (int i = 1; i < source.Length; i++)
            { 
                rawForce[i] = (source[i].Close - source[i - 1].Close) * source[i].Volume;
            }

            decimal?[] result = rawForce.Ema(options.Period).Result;
            
            return new SeriesIndicatorResult() { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, ForceIndexOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, ForceIndexOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}