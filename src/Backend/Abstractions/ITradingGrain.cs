using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface ITradingGrain : IGrainWithIntegerCompoundKey
	{
		Task<ITradingContext> GetContext();
	}
}