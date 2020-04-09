using System;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Grains;
using Orleans;
using Orleans.Concurrency;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	public class TradeProcessingGrain : Grain, ITradeProcessingGrain
	{
		public Task Process(ITrade trade)
		{
			throw new NotImplementedException();
		}
	}
}