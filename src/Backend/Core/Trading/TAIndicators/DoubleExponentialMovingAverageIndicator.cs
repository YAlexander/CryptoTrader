﻿using System.Linq;
using Contracts;
using Core.Trading.TAIndicators.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class DoubleExponentialMovingAverageIndicator : BaseIndicator<DemaOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Double Exponential Moving Average (DEMA) Indicator";
    
        public override SeriesIndicatorResult Get(decimal?[] source, DemaOptions options)
        {
            decimal?[] result = new decimal?[source.Length];
            decimal?[] emaFirst = source.Ema(options.Period).Result;
            decimal?[] emaSecond = emaFirst.Ema(options.Period).Result;

            for (int i = 0; i < source.Length; i++)
            {
                if (i >= options.Period)
                {
                    result[i] = 2 * emaFirst[i] - emaSecond[i];
                }
                else
                {
                    result[i] = null;
                }
            }
            
            return new SeriesIndicatorResult { Result = result };
        }

        public override SeriesIndicatorResult Get(ICandle[] source, DemaOptions options)
        {
            decimal[] values = source.Select(x => x.Close).ToArray();

            return Get(values, options);
        }

        public override SeriesIndicatorResult Get(decimal[] source, DemaOptions options)
        {
            decimal?[] values = source.Select(x => (decimal?) x).ToArray();

            return Get(values, options);
        }
    }
}