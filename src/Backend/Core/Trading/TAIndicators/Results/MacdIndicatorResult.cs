using Contracts.Trading;

namespace core.Trading.TAIndicators.Results
{
	public class MacdIndicatorResult : IResultSet
	{
		public decimal?[] Macd { get; set; }
		
		public decimal?[] Signal { get; set; }
		
		public decimal?[] Hist { get; set; }
	}
}