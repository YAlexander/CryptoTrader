using System;
using Contracts.Trading;

namespace Abstractions
{
	public interface IOrderNotification : INotification<IOrder>
	{
	}
}