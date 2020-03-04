using System;
using System.Collections.Generic;
using Contracts;
using TechanCore.Enums;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class RenkoIndicator : BaseIndicator<RenkoOptions, RenkoResult>
    {
        public override string Name { get; } = "Renko (RENKO) Indicator";
        
        public override RenkoResult Get(ICandle[] source, RenkoOptions options)
        {
            List<RenkoBrick> result = new List<RenkoBrick>();
            decimal?[] atr = source.Atr(options.AtrPeriod).Result;

            
            result.Add(new RenkoBrick()
            {
                Type = RenkoBrickType.WHITE,
                High = (int)source[options.AtrPeriod].High / atr[options.AtrPeriod].Value,
                Low = (int)source[options.AtrPeriod].Low / atr[options.AtrPeriod].Value,
                Price = source[options.AtrPeriod].Close,
                Time = source[options.AtrPeriod].Time
            });
            
            
            for (int i = options.AtrPeriod + 1; i < source.Length; i++)
            {
                int bricksCount = Math.Abs((int)((source[i].Close - source[i - 1].Close) / atr[i].Value));

                RenkoBrickType bricksColor;
                if (source[i].Close > source[i - 1].Close && bricksCount > 0)
                {
                    bricksColor = RenkoBrickType.WHITE;
                }
                else if (source[i].Close < source[i - 1].Close && bricksCount > 0)
                {
                    bricksColor = RenkoBrickType.BLACK;
                }
                else
                {
                    continue;
                }
                
                for (int j = 0; j < bricksCount; j++)
                {
                    RenkoBrick brick = new RenkoBrick();
                    brick.Time = source[i].Time;

                    // TODO: Review High/Low logic
                    if (j == bricksCount - 1)
                    {
                        brick.High = (int) (source[i].High / atr[i].Value);
                        brick.Low = (int) (source[i].Low / atr[i].Value);
                    }

                    if (bricksColor == RenkoBrickType.WHITE)
                    {
                        brick.Type = RenkoBrickType.WHITE;
                        brick.Price = result[i - 1].Price + atr[i].Value;
                    }
                    else
                    {
                        brick.Type = RenkoBrickType.WHITE;
                        brick.Price = result[i - 1].Price + atr[i].Value;
                    }
                    
                    result.Add(brick);
                }
            }
            
            return new RenkoResult { Bricks = result };
        }

        public override RenkoResult Get(decimal[] source, RenkoOptions options)
        {
            throw new NotImplementedException();
        }

        public override RenkoResult Get(decimal?[] source, RenkoOptions options)
        {
            throw new NotImplementedException();
        }
    }
}