using System.Collections.Generic;
using TechanCore.Strategies;

namespace TechanCore
{
	public class OrdersBook : IOrdersBook
	{
		public decimal LastPrice { get; set; }
		public IEnumerable<OrdersBookLine> BookLines { get; set; }
	}
}
