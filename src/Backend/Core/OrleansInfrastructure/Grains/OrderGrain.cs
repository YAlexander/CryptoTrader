using System.Threading.Tasks;
using Abstractions;
using Common.Trading;
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
		
		public async Task Receive(IOrder order)
		{
			_order.State = (Order) order;
			await _order.WriteStateAsync();
		}

		public async Task Update(IOrder order)
		{
			_order.State = (Order)order;
			await _order.WriteStateAsync();
		}
	}
}