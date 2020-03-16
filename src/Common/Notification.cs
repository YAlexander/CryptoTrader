using System;
using Abstractions;

namespace Common
{
	public class Notification<T> : INotification<T> where T : class
	{
		public DateTime Created { get; set; } = DateTime.Now;
		public DateTime? Expired { get; set; }
		public T Payload { get; set; }
	}
}