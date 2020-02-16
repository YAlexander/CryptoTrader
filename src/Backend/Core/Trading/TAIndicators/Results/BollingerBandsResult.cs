using Contracts.Trading;

namespace core.Trading.TAIndicators.Results
{
    public class BollingerBandsResult : IResultSet
    {
        public decimal?[] UpperBand { get; set; }
        public decimal?[] MiddleBand { get; set; }
        public decimal?[] LowerBand { get; set; }
    }
}