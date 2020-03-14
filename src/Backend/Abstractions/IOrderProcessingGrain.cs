using System.Threading.Tasks;
using Orleans;

namespace Abstractions
{
	public interface IOrderProcessingGrain : IGrainWithIntegerKey
	{
		Task Subscribe(INotificator observer);

		Task UnSubscribe(INotificator observer);
	}
}