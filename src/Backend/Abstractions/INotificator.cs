using Orleans;

namespace Abstractions
{
	public interface INotificator : IGrainObserver
	{
		void ReceiveMessage(INotification notification);
	}
}