using System;
using System.Linq;
using Contracts;
using Contracts.Enums;
using Core.Trading.TAIndicators.Extensions;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class MovingAverageEnvelopesIndicator : BaseIndicator<MovingAverageEnvelopesOptions, MovingAverageEnvelopesResult>
    {
        public override string Name { get; } = "Moving Average Envelopes (MAE) Indicator";
        
        public override MovingAverageEnvelopesResult Get(ICandle[] source, MovingAverageEnvelopesOptions options)
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

        public override MovingAverageEnvelopesResult Get(decimal[] source, MovingAverageEnvelopesOptions options)
        {
            decimal?[] values = source.Select(x => (decimal?) x).ToArray();
            return Get(values, options);
        }

        public override MovingAverageEnvelopesResult Get(decimal?[] source, MovingAverageEnvelopesOptions options)
        {
            decimal?[] sma = source.Sma(options.Period).Result;
            decimal?[] upperLine = new decimal?[source.Length];
            decimal?[] lowerLine = new decimal?[source.Length];
            
            for (int i = 0; i < source.Length; i++)
            {   
                if (sma[i].HasValue)
                {
                    lowerLine[i] = sma[i] - sma[i].Value * (decimal)options.DownwardFactor;
                    upperLine[i] = sma[i] + sma[i].Value * (decimal)options.UpwardFactor;
                }
                else
                {
                    lowerLine[i] = null;
                    upperLine[i] = null;
                }
            }
            
            return new MovingAverageEnvelopesResult()
            {
                LowerLine = lowerLine,
                UpperLine = upperLine,
                MiddleLine = sma
            };
        }
    }
}