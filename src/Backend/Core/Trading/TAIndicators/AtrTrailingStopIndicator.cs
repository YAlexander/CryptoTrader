using System;
using Contracts;
using Contracts.Enums;
using Core.Trading.TAIndicators.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class AtrTrailingStopIndicator : BaseIndicator<AtrTrailingStopOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "ATR Trailing Stop (ATR TS) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, AtrTrailingStopOptions options)
        {
            decimal?[] result = new decimal?[source.Length];
            decimal?[] ema = source.Ema(options.Period, CandleVariables.CLOSE).Result;
            decimal?[] atrs = source.Atr(options.Period).Result;

            for (int i = options.Period; i < source.Length; i++)
            {
                decimal? loss = atrs[i] * options.Multiplier;

                result[i] = source[i].Close > ema[i] ? source[i].Close - loss : source[i].Close + loss;
            }
            
            return new SeriesIndicatorResult() { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, AtrTrailingStopOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, AtrTrailingStopOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}