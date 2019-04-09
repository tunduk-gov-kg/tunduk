using System;

namespace Catalog.Domain.Entity
{
    public class MessageLifecycle
    {
        public DateTime? RequestInTs { get; set; }
        public DateTime? RequestOutTs { get; set; }
        public DateTime? ResponseInTs { get; set; }
        public DateTime? ResponseOutTs { get; set; }
    }
}