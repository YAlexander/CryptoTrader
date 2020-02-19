using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
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

            int weightSum = 0;
            for (int i = 1; i <= options.Period; i++)
            {
                weightSum += i;
            }
            
            for (int i = 0; i < source.Length; i++)
            {
                if (i >= options.Period - 1)
                {
                    decimal? wma = 0m;
                    int weight = 1;
                    
                    for (int j = i - (options.Period - 1); j <= i; j++)
                    {
                        wma += (weight / weightSum) * source[i];
                        weight++;
                    }
                    result[i] = wma;
                }
                else
                {
                    result[i] = null;
                }
            }
			
            return new SeriesIndicatorResult { Result = result};
        }
        
        public override SeriesIndicatorResult Get(ICandle[] source, WmaOptions options)
        {
            decimal[] values = options.CandleVariable switch
            {
                CandleVariables.CLOSE => source.Select(x => x.Close).ToArray(),
                CandleVariables.HIGH => source.Select(x => x.High).ToArray(),
                CandleVariables.LOW => source.Select(x => x.Low).ToArray(),
                CandleVariables.OPEN => source.Select(x => x.Open).ToArray(),
                _ => throw new Exception("Unknown CandleVariableCode")
            };

            return Get(values, options);
        }

        public override SeriesIndicatorResult Get(decimal[] source, WmaOptions options)
        {
            decimal?[] values = source.Select(x => (decimal?) x).ToArray();
            return Get(values, options);
        }
    }
}