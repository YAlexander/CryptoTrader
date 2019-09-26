using System;
using System.Collections.Generic;

namespace core.Infrastructure.Database.Entities
{
	public class Order : BaseEntity
	{
		public int TradingModeCode { get; set; }
		public int ExchangeCode { get; set; }

		public int OrderSideCode { get; set; }
		public int OrderTypeCode { get; set; }
		public int OrderStatusCode { get; set; }
		public int FillPoliticsCode { get; set; }

		public long? DealId { get; set; }
		public long? ExchangeOrderId { get; set; }
		public long? ExchangeOrderStatusCode { get; set; }

		public string Symbol { get; set; }

		public decimal? Price { get; set; }
		public decimal? Amount { get; set; }

		public decimal? Limit { get; set; }

		// Place limit order when Stop price is reached
		public decimal? StopLimit { get; set; }

		// Place market order when Stop Loss price is reached
		public decimal? StopLoss { get; set; }

		// Place market order when TakeProfit price is reached
		public decimal? TakeProfit { get; set; }

		public DateTime? ExpirationDate { get; set; }
		public DateTime? LastErrorDate { get; set; }
		public string LastError { get; set; }

		// Update order status on Exchange
		public bool UpdateRequired { get; set; }
		public bool CancelRequired { get; set; }
		public string StatusDescription { get; set; }

		public decimal? Fee { get; set; }
		public string Asset { get; set; }

		public List<Fill> Fills { get; } = new List<Fill>();
 	}
}
