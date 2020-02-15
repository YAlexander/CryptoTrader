using System;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class OnBalanceVolumeIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "On Balance Volume (OBV) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            decimal?[] result = new decimal?[source.Length];
            result[0] = source[0].Close;
            
            for (int i = 1; i < source.Length; i++)
            {   
                if (source[i].Close > source[i - 1].Close)
                {
                    result[i] = result[i - 1] + source[i].Volume;
                }
                else if (source[i].Close < source[i - 1].Close)
                {
                    result[i] = result[i - 1] - source[i].Volume;
                }
                else
                {
                    result[i] = result[i - 1];
                }
            }

            return new SeriesIndicatorResult() { Result = result };
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