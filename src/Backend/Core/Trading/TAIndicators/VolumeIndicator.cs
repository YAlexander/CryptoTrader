using System.Linq;
using Contracts;
using core.Trading.TAIndicators.Options;
using core.Trading.TAIndicators.Results;

namespace Core.Trading.TAIndicators
{
    public class VolumeIndicator : BaseIndicator<EmptyOption, SeriesIndicatorResult>
    {
        public override string Name { get; } = "Volume (VOL) Indicator";
        
        public override SeriesIndicatorResult Get(ICandle[] source, EmptyOption options)
        {
            return new SeriesIndicatorResult() { Result = source.Select(x => (decimal?)x.Volume).ToArray() };
        }

        public override SeriesIndicatorResult Get(decimal[] source, EmptyOption options)
        {
            throw new System.NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, EmptyOption options)
        {
            throw new System.NotImplementedException();
        }
    }
}