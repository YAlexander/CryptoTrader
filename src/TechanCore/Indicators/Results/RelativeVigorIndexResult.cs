using Contracts.Trading;

namespace TechanCore.Indicators.Results
{
    public class RelativeVigorIndexResult : IResultSet
    {
        public decimal?[] Rvi { get; set; }
        public decimal?[] Signal { get; set; }
    }
}