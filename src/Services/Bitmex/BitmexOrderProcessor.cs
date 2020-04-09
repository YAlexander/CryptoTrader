using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;

namespace Bitmex
{
	public class BitmexOrderProcessor : IExchangeOrderProcessor
	{
		public Task<string> PlaceOrder(IOrder order)
		{
			throw new System.NotImplementedException();
		}

		public Task<IOrder> UpdateOrder(IOrder order)
		{
			throw new System.NotImplementedException();
		}

		public Task CancelOrder(string id)
		{
			throw new System.NotImplementedException();
		}
	}
}