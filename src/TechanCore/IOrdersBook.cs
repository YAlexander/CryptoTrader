using System.Collections.Generic;

namespace TechanCore.Strategies
{
	public interface IOrdersBook
	{
		decimal LastPrice { get; set; }
		IEnumerable<OrdersBookLine> BookLines { get; set; }
	}
}
