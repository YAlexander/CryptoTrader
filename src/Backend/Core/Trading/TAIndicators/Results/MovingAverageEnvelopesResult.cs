using Contracts.Trading;

namespace core.Trading.TAIndicators.Results
{
    public class MovingAverageEnvelopesResult : IResultSet
    {
        public decimal?[] MiddleLine { get; set; }
        
        public decimal?[] UpperLine { get; set; }
        
        public decimal?[] LowerLine { get; set; }
    }
}