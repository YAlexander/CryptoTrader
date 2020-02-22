using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
    public class ParabolicSarOptions : IOptionsSet
    {
        public double AccelerationFactor { get; set; }
        public double MaximumAccelerationFactor { get; set; }
        public int Period { get; set; }
    }
}