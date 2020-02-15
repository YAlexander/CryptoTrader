using Contracts.Trading;

namespace core.Trading.TAIndicators.Options
{
    public class MomentumOptions : IOptionsSet
    {
        public int Period { get; set; }
    }
}