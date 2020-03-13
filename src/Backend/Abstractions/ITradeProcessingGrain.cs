using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface ITradeProcessingGrain : IGrainWithIntegerCompoundKey
	{
		Task Set(ITrade trade);
	}
}