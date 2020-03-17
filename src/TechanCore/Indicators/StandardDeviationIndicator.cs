﻿using System;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class StandardDeviationIndicator : BaseIndicator<StandardDeviationOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Standard Deviation (StdDev) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, StandardDeviationOptions options)
        {
            decimal[] values = options.CandleVariable switch
            {
                CandleVariables.CLOSE => source.Close(),
                CandleVariables.HIGH => source.High(),
                CandleVariables.LOW => source.Low(),
                CandleVariables.OPEN => source.Open(),
                _ => throw new Exception("Unknown CandleVariableCode")
            };

            return Get(values, options);
        }

        public override SeriesIndicatorResult Get(decimal[] source, StandardDeviationOptions options)
        {
            decimal?[] values = source.ToNullable();
            return Get(values, options);
        }

        public override SeriesIndicatorResult Get(decimal?[] source, StandardDeviationOptions options)
        {
            decimal?[] middleBand = source.Sma(options.Period).Result;
            decimal?[] result = new decimal?[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                if (i >= options.Period - 1)
                {
                    double stdev =
                        Math.Sqrt(source
                            .Skip(i - options.Period)
                            .Take(options.Period)
                            .Select(x =>
                                (double)((x - middleBand[i].Value) * (x - middleBand[i].Value)))
                            .Sum() / (options.Period - 1));

                    result[i] = (decimal) stdev;
                }
                else
                {
                    result[i] = null;
                }
            }
            
            return new SeriesIndicatorResult { Result = result };
        }
    }
}