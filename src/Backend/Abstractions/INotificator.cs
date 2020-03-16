using System.Threading.Tasks;
using Orleans;

namespace Abstractions
{
	public interface INotificator<T> : IGrainObserver where T : class
	{
		Task ReceiveMessage(INotification<T> notification);
	}
}