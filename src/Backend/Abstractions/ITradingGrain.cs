using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface ITradingGrain : IGrainWithGuidCompoundKey
	{
		Task<ITradingContext> GetContext();
	}
}