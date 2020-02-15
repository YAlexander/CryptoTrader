using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class ChandeMomentumOscillatorIndicator : BaseIndicator<CmoOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Chande Momentum Oscillator (CMO) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, CmoOptions options)
        {
            decimal?[] result = new decimal?[source.Length];
            
            decimal?[] upValues = new decimal?[source.Length];
            upValues[0] = 0;
            decimal?[] downValues = new decimal?[source.Length];
            downValues[0] = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (source[i].Close > source[i - 1].Close)
                {
                    upValues[i] = source[i].Close - source[i - 1].Close;
                    downValues[i] = 0;        
                }
                else if (source[i].Close < source[i - 1].Close)
                {
                    upValues[i] = 0;
                    downValues[i] = source[i - 1].Close - source[i].Close;
                }
                else
                {
                    upValues[i] = 0;
                    downValues[i] =0;
                }

                if (i >= options.Period)
                {
                    decimal? upTotal = 0m, downTotal = 0m;
                    
                    for (int j = i; j >= i - (options.Period - 1); j--)
                    {
                        upTotal += upValues[j];
                        downTotal += downValues[j];
                    }
                    
                    result[i] = 100 * (upTotal - downTotal) / (upTotal + downTotal);
                }
                else
                {
                    result[i] = null;
                }
            }
            
            return new SeriesIndicatorResult() { Result = result };
        }

        public override SeriesIndicatorResult Get(decimal[] source, CmoOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, CmoOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}