using System;
using System.Threading.Tasks;
using Abstractions;

namespace Common
{
	public abstract class Notificator<T> : INotificator<T> where T : class
	{
		public Task ReceiveMessage(INotification<T> notification)
		{
			throw new NotImplementedException();
		}
	}
}