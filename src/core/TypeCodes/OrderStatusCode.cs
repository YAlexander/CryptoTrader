using core.Abstractions.TypeCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace core.TypeCodes
{
	public class OrderStatusCode : TypeCodeBase<int, OrderStatusCode>, IOrderStatusCode
	{
		public OrderStatusCode (int code, string description) : base(code, description)
		{
		}

		public static OrderStatusCode PENDING { get; } = new OrderStatusCode(1, "PENDING");
		public static OrderStatusCode LISTED { get; } = new OrderStatusCode(2, "LISTED");
		public static OrderStatusCode CLOSED { get; } = new OrderStatusCode(3, "CLOSED");
		public static OrderStatusCode CANCELED { get; } = new OrderStatusCode(4, "CANCELED");
		public static OrderStatusCode ERROR { get; } = new OrderStatusCode(5, "ERROR");
		public static OrderStatusCode EXPIRED { get; } = new OrderStatusCode(6, "EXPIRED");
		public static OrderStatusCode FILLED { get; } = new OrderStatusCode(7, "FILLED");
		public static OrderStatusCode PARTIALLY_FILLED { get; } = new OrderStatusCode(8, "PARTIALLY_FILLED");
		public static OrderStatusCode HOLD { get; } = new OrderStatusCode(1, "ON HOLD");
	}
}
