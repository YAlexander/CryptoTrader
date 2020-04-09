using System;
using System.Collections.Generic;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Persistence.Entities
{
	[Serializable]
	public class Order : BaseEntity, IOrder
	{
		public Guid OrderId { get; set; }
		public long? ParentOrderId { get; set; }
		public long DealId { get; set; }
		
		public string ExchangeOrderId { get; set; }
		
		public OrderStatus Status { get; set; }
		public DateTime? Expires { get; set; }

		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }

		public OrderSides OrderSide { get; set; }
		public OrderType OrderType { get; set; }
		public FillPolitics Politics { get; set; }
		
		public bool IsVirtual { get; set; }
		
		public decimal Amount { get; set; }
		public decimal? Limit { get; set; }
		public decimal? StopLimit { get; set; }
		public decimal? StopLoss { get; set; }
		public decimal? TakeProfit { get; set; }
		
		public List<IOrder> ChildOrders { get; set; } = new List<IOrder>();
		public List<ITrade> Trades { get; set; } = new List<ITrade>();
		
		public bool CreateRequired { get; set; }
		public bool UpdateRequired { get; set; }
		public bool CancelRequired { get; set; }
		
		public string ErrorDetails { get; set; }
	}
}