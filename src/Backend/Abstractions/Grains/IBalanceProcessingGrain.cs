using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface IBalanceProcessingGrain : IGrainWithIntegerKey
	{
		Task<IEnumerable<IBalance>> Get();

		Task Update(IBalance balance);
	}
}