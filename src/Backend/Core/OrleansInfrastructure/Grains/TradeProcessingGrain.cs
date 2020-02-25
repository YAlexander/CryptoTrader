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
	public class TradeProcessingGrain : Grain, ITradeProcessingGrain
	{
		public Task<bool> Receive(ITradeInfo trade)
		{
			throw new NotImplementedException();
		}
	}
}