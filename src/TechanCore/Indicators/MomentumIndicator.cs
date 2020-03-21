using System.Linq;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class MomentumIndicator : BaseIndicator<MomentumOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Momentum (MOM) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, MomentumOptions options)
        {
            decimal?[] result = new decimal?[source.Length];

            for (int i = options.Period; i < source.Length; i++)
            {
                result[i] = source[i].Close - source[i - options.Period].Close;    
            }

            return new SeriesIndicatorResult {Result = result};
        }

        public override SeriesIndicatorResult Get(decimal[] source, MomentumOptions options)
        {
            decimal?[] values = source.Select(x => (decimal?) x).ToArray();
            return Get(values, options);
        }

        public override SeriesIndicatorResult Get(decimal?[] source, MomentumOptions options)
        {
            decimal?[] result = new decimal?[source.Length];

            for (int i = options.Period; i < source.Length; i++)
            {
                result[i] = source[i] - source[i - options.Period];    
            }

            return new SeriesIndicatorResult { Result = result };
        }
    }
}