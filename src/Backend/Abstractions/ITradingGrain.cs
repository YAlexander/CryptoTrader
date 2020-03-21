using System.Threading.Tasks;
using Orleans;

namespace Abstractions
{
	public interface ITradingGrain : IGrainWithGuidCompoundKey
	{
		Task<ITradingContext> GetContext();
	}
}