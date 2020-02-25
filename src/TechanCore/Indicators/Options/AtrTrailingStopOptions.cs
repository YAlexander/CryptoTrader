using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
    public class AtrTrailingStopOptions : IOptionsSet
    {
        public int Period { get; set; }
        
        public decimal Multiplier { get; set; }
    }
}