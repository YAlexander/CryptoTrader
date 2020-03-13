using System;
using System.Threading.Tasks;
using Abstractions;
using Contracts.Trading;
using Orleans;
using Orleans.Concurrency;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	public class TradeProcessingGrain : Grain, ITradeProcessingGrain
	{
		public Task Set(ITrade trade)
		{
			throw new NotImplementedException();
		}
	}
}