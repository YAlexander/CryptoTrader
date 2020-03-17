using System.Threading.Tasks;
using Orleans;

namespace Abstractions
{
	public interface INotificationGrain : IGrainWithIntegerKey
	{
		Task Subscribe(IOrderNotificator observer);

		Task UnSubscribe(IOrderNotificator observer);
	}
}