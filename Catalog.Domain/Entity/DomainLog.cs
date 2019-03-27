using Catalog.Domain.Enum;

namespace Catalog.Domain.Entity {
    public class DomainLog : BaseEntity {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}