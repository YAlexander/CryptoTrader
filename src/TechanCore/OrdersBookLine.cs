using TechanCore.Enums;

namespace TechanCore
{
	public class OrdersBookLine
	{
		private PriceType Type { get; set; }
		public decimal Price { get; set; }
		public decimal Amount { get; set; }
	}
}
