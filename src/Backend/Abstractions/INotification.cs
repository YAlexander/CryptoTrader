using System;

namespace Abstractions
{
	public interface INotification<T>
	{
		DateTime Created { get; set; }
		DateTime? Expired { get; set; }
		
		T Payload { get; set; }
	}
}