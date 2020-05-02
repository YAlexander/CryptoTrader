using System.Threading.Tasks;
using Abstractions.Entities;
using Abstractions.Enums;
using Orleans;

namespace Abstractions.Grains
{
	public interface ITradeProcessingGrain : IGrainWithIntegerKey
	{
		Task Process(ITrade trade);

		Task<decimal?> Get(Assets asset1, Assets asset2);
	}
}