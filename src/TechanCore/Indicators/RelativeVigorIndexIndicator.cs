using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class RelativeVigorIndexIndicator : BaseIndicator<EmptyOption, RelativeVigorIndexResult>
    {
        public override string Name { get; } = "Relative Vigor Index (RVI) Index";

        public override RelativeVigorIndexResult Get(ICandle[] source, EmptyOption options)
        {
            decimal?[] rvi = new decimal?[source.Length];
            decimal?[] signal = new decimal?[source.Length];

            for (int i = 3; i < source.Length; i++)
            {
                decimal movAvg = (source[i].Close - source[i].Open)
                                 + 2 * (source[i - 1].Close - source[i - 1].Open)
                                 + 2 * (source[i - 2].Close - source[i - 2].Open)
                                 + (source[i - 3].Close - source[i - 3].Open);

                decimal rangeAvg = (source[i].High - source[i].Low)
                                   + 2 * (source[i - 1].High - source[i - 1].Low)
                                   + 2 * (source[i - 2].High - source[i - 2].Low)
                                   + (source[i - 3].High - source[i - 3].Low);

                rvi[i] = movAvg / rangeAvg;

                if (i >= 6)
                {
                    signal[i] = (rvi[i] + 2 * rvi[i - 1] + 2 * rvi[i - 2] + rvi[i - 3]) / 6;
                }
            }
            
            return new RelativeVigorIndexResult { Rvi = rvi, Signal = signal };
        }
        
        public override RelativeVigorIndexResult Get(decimal[] source, EmptyOption options)
        {
            throw new System.NotImplementedException();
        }

        public override RelativeVigorIndexResult Get(decimal?[] source, EmptyOption options)
        {
            throw new System.NotImplementedException();
        }
    }
}