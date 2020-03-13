using System.Threading.Tasks;
using Abstractions;
using Contracts.Trading;
using Orleans;
using Orleans.Runtime;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains
{
	public class OrderGrain : Grain, IOrderGrain
	{
		private readonly IPersistentState<Order> _order;

		public OrderGrain(
			[PersistentState(nameof(Order), "orderStore")] IPersistentState<Order> order
		)
		{
			_order = order;
		}

		public Task<bool> Receive(IOrder order)
		{
			throw new System.NotImplementedException();
		}
	}
}