using System;

namespace Persistence.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}