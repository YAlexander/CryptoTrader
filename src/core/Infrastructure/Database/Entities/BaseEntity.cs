using core.Abstractions;
using System;

namespace core.Infrastructure.Database.Entities
{
	public class BaseEntity : IEntity
	{
		public long Id { get; set; }

		public DateTime Created { get; } = DateTime.UtcNow;
		public DateTime? Updated { get; set; }

		public bool IsDeleted { get; set; }
	}
}
