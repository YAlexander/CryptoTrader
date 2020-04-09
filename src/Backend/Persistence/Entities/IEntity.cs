using System;

namespace Persistence.Entities
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}