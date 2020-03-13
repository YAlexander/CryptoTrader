using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface ITradeProcessingGrain : IGrainWithIntegerKey
	{
		Task Receive(ITrade trade);
	}
}