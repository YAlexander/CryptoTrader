using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Grains;
using Orleans;
using Orleans.Runtime;
using Persistence.Entities;
using Persistence.PostgreSQL.Providers;

namespace Core.OrleansInfrastructure.Grains
{
	public class OrderGrain : Grain, IOrderGrain
	{
		private readonly IPersistentState<Order> _order;

		public OrderGrain(
			[PersistentState(nameof(Order), nameof(OrdersStorageProvider))] IPersistentState<Order> order
		)
		{
			_order = order;
		}
		
		public Task<IOrder> Get()
		{
			return Task.FromResult((IOrder)_order.State);
		}

		public async Task Update(IOrder order)
		{
			_order.State = (Order)order;
			await _order.WriteStateAsync();
		}
	}
}