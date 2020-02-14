using Contracts.Enums;
using Contracts.Trading;

namespace core.Trading.TAIndicators.Options
{
    public class WmaOptions : IOptionsSet
    {
        public int Period { get; set; }

        public CandleVariables? CandleVariable { get; set; }
    }
}