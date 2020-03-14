using System;

namespace Abstractions
{
	public interface INotification
	{
		DateTime Created { get; set; }
		DateTime? Expired { get; set; }
		
		object Payload { get; set; }
	}
}