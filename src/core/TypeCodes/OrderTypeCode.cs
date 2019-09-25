using core.Abstractions.TypeCodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace core.TypeCodes
{
	public class OrderTypeCode : TypeCodeBase<int, OrderTypeCode>, IOrderTypeCode
	{
		public OrderTypeCode (int code, string description) : base(code, description)
		{
		}

		public static OrderTypeCode UNKNOWN { get; } = new OrderTypeCode(0, "UNKNOWN");

		public static OrderTypeCode MKT { get; } = new OrderTypeCode(1, "MARKET");
		public static OrderTypeCode LMT { get; } = new OrderTypeCode(2, "LIMIT");
		public static OrderTypeCode LST { get; } = new OrderTypeCode(3, "STOP LIMIT");
		public static OrderTypeCode TRL { get; } = new OrderTypeCode(4, "TRAILING");
		public static OrderTypeCode SMT { get; } = new OrderTypeCode(5, "STOP MARKET");
	}
}
