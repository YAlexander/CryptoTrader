﻿using System;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class AccelerationDecelerationIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Acceleration/Deceleration (AC) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            // 5 and 34 - values, recommended by indicator's author 
            decimal?[] awesomeValues = source.AwesomeOscillator(5, 34).Result;
            decimal?[] sma = awesomeValues.Sma(5).Result;
            
            decimal?[] result = new decimal?[source.Length];

            for (int i = 5; i < source.Length; i++)
            {
                result[i] = awesomeValues[i] - sma[i];
            }
            
            return new SeriesIndicatorResult { Result = result };
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