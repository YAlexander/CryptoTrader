using Contracts.Enums;
using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
    public class TemaOptions : IOptionsSet
    {
        public int Period { get; set; }
        
        public CandleVariables? CandleVariable { get; set;  } 
    }
}