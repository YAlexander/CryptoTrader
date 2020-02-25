using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
    public class MomentumOptions : IOptionsSet
    {
        public int Period { get; set; }
    }
}