using System.Threading.Tasks;
using Abstractions;
using Contracts.Trading;
using Core.OrleansInfrastructure.Grains.GrainStates;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	public class OrderReceiver : Grain, IOrderReceiver
	{
		private readonly IPersistentState<OrderState> _order;
		
		public OrderReceiver([PersistentState(nameof(order), nameof(OrderState))] IPersistentState<OrderState> order)
		{
		 	_order = order;
		 }

		public Task<bool> Receive(IOrder order)
		{
			throw new System.NotImplementedException();
		}
	}
}