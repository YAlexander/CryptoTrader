using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class DoubleExponentialMovingAverageIndicator : BaseIndicator<DemaOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Double Exponential Moving Average (DEMA) Indicator";
    
        public override SeriesIndicatorResult Get(decimal?[] source, DemaOptions options)
        {
            decimal?[] result = new decimal?[source.Length];
            decimal?[] emaFirst = source.Ema(options.Period).Result;
            decimal?[] emaSecond = emaFirst.Ema(options.Period).Result;

            for (int i = 0; i < source.Length; i++)
            {
                if (emaFirst[i].HasValue && emaSecond[i].HasValue)
                {
                    result[i] = 2 * emaFirst[i].Value - emaSecond[i].Value;
                }
            }
            
            return new SeriesIndicatorResult { Result = result };
        }

        public override SeriesIndicatorResult Get(ICandle[] source, DemaOptions options)
        {
            decimal[] values = source.Close();
            return Get(values, options);
        }

        public override SeriesIndicatorResult Get(decimal[] source, DemaOptions options)
        {
            decimal?[] values = source.ToNullable();
            return Get(values, options);
        }
    }
}