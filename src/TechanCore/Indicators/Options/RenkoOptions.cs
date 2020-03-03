using Contracts.Trading;

namespace TechanCore.Indicators.Options
{
    public class RenkoOptions : IOptionsSet
    {
        public int AtrPeriod { get; set; }
    }
}