using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface IOrderProcessingGrain : IGrainWithIntegerKey
	{
		Task Subscribe(IOrderNotificator observer);

		Task UnSubscribe(IOrderNotificator observer);
	}
}