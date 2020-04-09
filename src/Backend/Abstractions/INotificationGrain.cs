﻿using System.Threading.Tasks;
using Orleans;

namespace Abstractions
{
	public interface INotificationService
	{
		Task Subscribe(IOrderNotificator observer);

		Task UnSubscribe(IOrderNotificator observer);
	}
}