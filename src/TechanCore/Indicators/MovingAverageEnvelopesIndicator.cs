using System;
using System.Linq;
using Contracts;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class MovingAverageEnvelopesIndicator : BaseIndicator<MovingAverageEnvelopesOptions, MovingAverageEnvelopesResult>
    {
        public override string Name { get; } = "Moving Average Envelopes (MAE) Indicator";
        
        public override MovingAverageEnvelopesResult Get(ICandle[] source, MovingAverageEnvelopesOptions options)
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

        public override MovingAverageEnvelopesResult Get(decimal[] source, MovingAverageEnvelopesOptions options)
        {
            decimal?[] values = source.ToNullable();
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
            
            return new MovingAverageEnvelopesResult
            {
                LowerLine = lowerLine,
                UpperLine = upperLine,
                MiddleLine = sma
            };
        }
    }
}