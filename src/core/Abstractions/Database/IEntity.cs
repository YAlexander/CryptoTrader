using System;

namespace core.Abstractions
{
	public interface IEntity
	{
		long Id { get; set; }
		DateTime Created { get; }
		bool IsDeleted { get; set; }
	}
}
