using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class TripleExponentialMovingAverageIndicator : BaseIndicator<TemaOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Triple Exponential Moving Average (TEMA) Indicator";
    
        public override SeriesIndicatorResult Get(decimal?[] source, TemaOptions options)
        {
            decimal?[] firstEma = source.Ema(options.Period).Result;
            decimal?[] secondEma = firstEma.Ema(options.Period).Result;
            decimal?[] thirdEma = secondEma.Ema(options.Period).Result;
            
            decimal?[] result = new decimal?[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                if (i >= options.Period)
                {
                    result[i] = 3 * firstEma[i] - 3 * secondEma[i] + thirdEma[i];
                }
                else
                {
                    result[i] = null;
                }
            }
            
            return new SeriesIndicatorResult { Result = result };
        }

        public override SeriesIndicatorResult Get(ICandle[] source, TemaOptions options)
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

        public override SeriesIndicatorResult Get(decimal[] source, TemaOptions options)
        {
            decimal?[] valuses = source.Select(x => (decimal?) x).ToArray();

            return Get(valuses, options);
        }
    }
}