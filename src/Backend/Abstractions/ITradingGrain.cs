using System.Threading.Tasks;
using Abstractions.Enums;
using Orleans;

namespace Abstractions
{
	public interface ITradingGrain : IGrainWithGuidKey
	{
		Task<ITradingContext> GetContext(Exchanges exchange, Assets asset1, Assets asset2);
	}
}