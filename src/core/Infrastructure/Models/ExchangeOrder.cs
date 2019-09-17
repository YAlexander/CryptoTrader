using core.Abstractions.TypeCodes;
using System;

namespace core.Infrastructure.Models
{
	public class ExchangeOrder
	{
		public long OrderId { get; set; }
		public long? DealId { get; set; }
		public IOrderStatusCode Status { get; set; }
		public string ExchangeOrderId { get; set; }
		public IOrderSideCode OrderSideCode { get; set; }
		public IOrderTypeCode OrderTypeCode { get; set; }
		public IFillPoliticsCode FillPoliticsCode { get; set; }
		public decimal ExecutedQuantity { get; set; }
		public decimal Price { get; set; }
		public DateTime? TransactTime { get; set; }
		public string Symbol { get; set; }
		public decimal? OriginalQuantity { get; set; }
	}
}
