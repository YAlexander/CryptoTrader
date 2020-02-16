using Contracts.Trading;

namespace core.Trading.TAIndicators.Options
{
    public class BollingerBandsOptions : IOptionsSet
    {
        public int Period { get; set; }
        
        public double DeviationUp { get; set; }
        
        public double DeviationDown { get; set; }
    }
}