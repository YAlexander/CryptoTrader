using System;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class BollingerBandsIndicator : BaseIndicator<BollingerBandsOptions, BollingerBandsResult>
    {
        public override string Name { get; } = "Bollinger Bands (BB) Indicator";
        
        public override BollingerBandsResult Get(ICandle[] source, BollingerBandsOptions options)
        {
            decimal[] typicalPrice = source.Select(x => (x.High + x.Low + x.Close) / 3).ToArray();
            decimal?[] stdDevs = typicalPrice.StDev(options.Period).Result;
            
            decimal?[] middleBand = typicalPrice.Sma(options.Period).Result;

            decimal?[] upperBand = new decimal?[source.Length];
            decimal?[] lowerBand = new decimal?[source.Length];
            
            for (int i = 0; i < source.Length; i++)
            {
                if (i >= options.Period - 1)
                {
                    upperBand[i] = middleBand[i] + (decimal)options.DeviationUp * stdDevs[i];
                    lowerBand[i] = middleBand[i] + (decimal)options.DeviationDown * stdDevs[i];
                }
                else
                {
                    upperBand[i] = null;
                    lowerBand[i] = null;
                }
            }

            return new BollingerBandsResult
            {
                UpperBand = upperBand,
                MiddleBand = middleBand,
                LowerBand = lowerBand
            };
        }

        public override BollingerBandsResult Get(decimal[] source, BollingerBandsOptions options)
        {
            throw new NotImplementedException();
        }

        public override BollingerBandsResult Get(decimal?[] source, BollingerBandsOptions options)
        {
            throw new NotImplementedException();
        }
    }
}