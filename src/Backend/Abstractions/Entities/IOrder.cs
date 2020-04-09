using System;
using System.Collections.Generic;
using Abstractions.Enums;

namespace Abstractions.Entities
{
	public interface IOrder
	{
		public Guid Id { get; set; }
		long? ParentOrderId { get; set; }
		long DealId { get; set; }
		
		string ExchangeOrderId { get; set; }
		
		OrderStatus Status { get; set; }
		DateTime? Expires { get; set; }

		Exchanges Exchange { get; set; }
		Assets Asset1 { get; set; }
		Assets Asset2 { get; set; }

		OrderSides OrderSide { get; set; }
		OrderType OrderType { get; set; }
		FillPolitics Politics { get; set; }
		
		bool IsVirtual { get; set; }
		
		decimal Amount { get; set; }
		decimal? Limit { get; set; }
		decimal? StopLimit { get; set; }
		decimal? StopLoss { get; set; }
		decimal? TakeProfit { get; set; }
		
		List<IOrder> ChildOrders { get; set; }
		List<ITrade> Trades { get; set; }
		
		bool CreateRequired { get; set; }
		bool UpdateRequired { get; set; }
		bool CancelRequired { get; set; }
		string ErrorDetails { get; set; }
	}
}