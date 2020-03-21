using System;
using System.Linq;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;


namespace TechanCore.Indicators
{
    public class WmaIndicator : BaseIndicator<WmaOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Weighted Moving Average (WMA) Indicator";
        
        public override SeriesIndicatorResult Get(decimal?[] source, WmaOptions options)
        {
            decimal?[] result = new decimal?[source.Length];

            int weightSum = Enumerable.Range(0, options.Period).Sum();
            
            for (int i = 0; i < source.Length; i++)
            {
                if (i >= options.Period)
                {
                    decimal? wma = 0m;
                    int weight = 1;
                    
                    for (int j = i - options.Period; j < i; j--)
                    {
                        wma += (weight / weightSum) * source[i];
                        weight++;
                    }
                    
                    result[i] = wma;
                }
            }
			
            return new SeriesIndicatorResult { Result = result};
        }
        
        public override SeriesIndicatorResult Get(ICandle[] source, WmaOptions options)
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

        public override SeriesIndicatorResult Get(decimal[] source, WmaOptions options)
        {
            decimal?[] values = source.ToNullable();
            return Get(values, options);
        }
    }
}