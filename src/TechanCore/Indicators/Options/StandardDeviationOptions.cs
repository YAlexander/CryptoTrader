using Contracts.Enums;
using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
    public class StandardDeviationOptions : IOptionsSet
    {
        public int Period { get; set; }
        
        public CandleVariables? CandleVariable { get; set; }
    }
}