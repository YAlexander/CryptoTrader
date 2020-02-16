using Contracts;
using Core.Trading.TAIndicators.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class AccelerationDecelerationIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Acceleration/Deceleration (AC) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            decimal?[] awesomeValuse = source.AwesomeOscillator(5, 34).Result;
            decimal?[] sma = awesomeValuse.Sma(5).Result;
            
            decimal?[] result = new decimal?[source.Length];

            for (int i = 5; i < source.Length; i++)
            {
                result[i] = awesomeValuse[i] - sma[i];
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