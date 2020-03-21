using System;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class AtrTrailingStopIndicator : BaseIndicator<AtrTrailingStopOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "ATR Trailing Stop (ATR TS) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, AtrTrailingStopOptions options)
        {
            decimal?[] result = new decimal?[source.Length];
            decimal?[] atrs = source.Atr(options.Period).Result;

            for (int i = 1; i < source.Length; i++)
            {
                decimal? loss = atrs[i] * options.Multiplier;
                
                if (source[i].Close > result[i - 1] && source[i - 1].Close > result[i - 1])
                {
                    result[i] = result[i - 1] > source[i].Close - loss ? result[i - 1] : source[i].Close - loss;
                }
                else if (source[i].Close < result[i - 1] && source[i - 1].Close < result[i - 1])
                {
                    result[i] = result[i - 1] < source[i].Close + loss ? source[i].Close + loss : result[i - 1]; 
                }
                else if (source[i].Close > result[i - 1])
                {
                    result[i] = source[i].Close - loss;
                }
                else
                {
                    result[i] = source[i].Close + loss;
                }
            }
            
            return new SeriesIndicatorResult { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, AtrTrailingStopOptions options)
        {
            throw new NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, AtrTrailingStopOptions options)
        {
            throw new NotImplementedException();
        }
    }
}