using Contracts.Trading;

namespace core.Trading.TAIndicators.Options
{
    public class AtrTrailingStopOptions : IOptionsSet
    
    {
        public int Period { get; set; }
        
        public decimal Multiplier { get; set; }
    }
}