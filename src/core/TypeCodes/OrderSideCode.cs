using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class OrderSideCode : TypeCodeBase<int, OrderSideCode>, IOrderSideCode
	{
		public OrderSideCode (int code, string description) : base(code, description)
		{
		}

		public static OrderSideCode BUY { get; } = new OrderSideCode(0, "BUY");
		public static OrderSideCode SELL { get; } = new OrderSideCode(1, "SELL");
	}
}
