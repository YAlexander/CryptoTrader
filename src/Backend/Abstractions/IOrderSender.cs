using System.Threading.Tasks;
using Common.Trading;

namespace Abstractions
{
	public interface IExchangeOrderProcessor
	{
		/// <summary>
		/// Place order
		/// </summary>
		/// <param name="order">Order to place on Exchange</param>
		/// <returns>Exchange order Id</returns>
		Task<string> PlaceOrder(IOrder order);
		
		/// <summary>
		/// Update Order on Exchange
		/// </summary>
		Task<IOrder> UpdateOrder(IOrder order);
		
		/// <summary>
		/// Cancel Order
		/// </summary>
		/// <param name="id">Exchange Order Id</param>
		Task CancelOrder(string id);
	}
}