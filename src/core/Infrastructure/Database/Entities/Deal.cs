namespace core.Infrastructure.Database.Entities
{
	public class Deal : BaseEntity
	{
		public int StatusCode { get; set; }
		public int Exchange { get; set; }
		public string Symbol { get; set; }
		public decimal? AvgOpenPrice { get; set; }
		public decimal? AvgClosePrice { get; set; }
		public decimal? Amount { get; set; }
		public decimal? EstimatedFee { get; set; }

		// Set after BuyOrder is filled
		public decimal? StopLoss { get; set; }
		public decimal? TakeProfit { get; set; }
	}
}
