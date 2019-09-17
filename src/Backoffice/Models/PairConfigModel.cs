namespace Backoffice.Models
{
	public class PairConfigModel
	{
		public long Id { get; set; }
		public int ExchangeCode { get; set; }
		public string Symbol { get; set; }
		public bool IsEnabled { get; set; }
		public int? StrategyId { get; set; }
		public int? RefreshDelaySeconds { get; set; }
		public float? DefaultStopLossPercent { get; set; }
		public float? DefaultTakeProfitPercent { get; set; }
		public bool IsTestMode { get; set; }
	}
}
