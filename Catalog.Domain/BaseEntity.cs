using System;

namespace Catalog.Domain
{
    [Serializable]
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}