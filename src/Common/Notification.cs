using System;
using Abstractions;

namespace Common
{
	public class Notification : INotification
	{
		public DateTime Created { get; set; } = DateTime.Now;
		public DateTime? Expired { get; set; }
		public object Payload { get; set; }
	}
}