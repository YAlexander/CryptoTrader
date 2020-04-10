using System;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;
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

		public Task<decimal?> Get(Assets asset1, Assets asset2)
		{
			throw new NotImplementedException();
		}
	}
}