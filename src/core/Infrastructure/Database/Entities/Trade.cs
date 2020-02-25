using System;

namespace core.Infrastructure.Database.Entities
{
	public class Trade : BaseEntity
	{
		public DateTime Time { get; set; }
		public int ExchangeCode { get; set; }
		public string Symbol { get; set; }
		public long TradeId { get; set; }
		public decimal Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
