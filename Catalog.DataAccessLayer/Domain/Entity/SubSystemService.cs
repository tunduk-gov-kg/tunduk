using System;
using Catalog.DataAccessLayer.Domain.Entity.Interfaces;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class SubSystemService : IDateTimeTrackable, INonDeletable {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }
        public string Wsdl { get; set; }
        public SubSystem SubSystem { get; set; }
        public long SubSystemId { get; set; }
    }
}