using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Entities;
using Abstractions.Grains;
using Orleans;

namespace Core.OrleansInfrastructure.Grains
{
	public class BalanceProcessingGrain : Grain, IBalanceProcessingGrain
	{
		public Task<IEnumerable<IBalance>> Get()
		{
			throw new System.NotImplementedException();
		}

		public Task Update(IBalance balance)
		{
			throw new System.NotImplementedException();
		}
	}
}