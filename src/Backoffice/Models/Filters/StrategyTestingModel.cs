namespace Backoffice.Models.Filters
{
	public class StrategyTestingModel
	{
		public int StrategyId { get; set; }
		public int ExchangeCode { get; set; }
		public string Symbol { get; set; }
		public int IntervalCode { get; set; }
		public int NumberOfCandles { get; set; }
	}
}
