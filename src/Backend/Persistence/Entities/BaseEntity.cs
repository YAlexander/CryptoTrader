using System;

namespace Persistence.Entities
{
    public abstract class BaseEntity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}