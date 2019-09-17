using core.Abstractions.TypeCodes;
using core.TypeCodes;
using System;

namespace core.Infrastructure.Notifications
{
	public class Notification<T> where T: class
	{
		public DateTime Created { get; } = DateTime.UtcNow;

		public int Code { get; set; }

		public T Payload { get; set; }
	}
}
