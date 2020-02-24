using System;
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
	public class TradeReceiver : Grain, ITradeReceiver
	{
		private readonly IPersistentState<TradeState> _trade;
		
		 public TradeReceiver([PersistentState(nameof(trade), nameof(TradeState))] IPersistentState<TradeState> trade)
		 {
		 	_trade = trade;
		 }

		public Task<bool> Receive(ITradeInfo trade)
		{
			throw new NotImplementedException();
		}
	}
}