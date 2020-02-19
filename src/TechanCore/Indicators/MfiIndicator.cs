using System;
using System.Linq;
using Contracts;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

namespace TechanCore.Indicators
{
    public class MfiIndicator : BaseIndicator<MfiOptions, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Money Flow Index (MFI) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, MfiOptions options)
        {
            decimal[] typicalPrices = source.Select(x => (x.High + x.Close + x.Low) / 3).ToArray();
            
            decimal[] moneyFlows = new decimal[source.Length];
            moneyFlows[0] = typicalPrices[0] * source[0].Volume;
                
            decimal?[] mfiResult = new decimal?[source.Length];
            
            for (int i = 1; i < source.Length; i++)
            {
                moneyFlows[i] = typicalPrices[i] > typicalPrices[i - 1]
                    ? typicalPrices[i] * source[i].Volume
                    : -1 * typicalPrices[i] * source[i].Volume;

                if (i >= options.Period)
                {
                    decimal[] moneyFlowsByPeriods = moneyFlows.Skip(i - options.Period).Take(options.Period).ToArray();
                    decimal positiveFlow = moneyFlowsByPeriods.Where(x => x >= 0).Sum();
                    decimal negativeFlow = -1 * moneyFlowsByPeriods.Where(x => x < 0).Sum();

                    mfiResult[i] = 100 - 100 / (1 + positiveFlow / negativeFlow);
                }
                else
                {
                    mfiResult[i] = null;
                }
            }
            
            return new SeriesIndicatorResult { Result = mfiResult };
        }

        public override SeriesIndicatorResult Get(decimal[] source, MfiOptions options)
        {
            throw new NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, MfiOptions options)
        {
            throw new NotImplementedException();
        }
    }
}