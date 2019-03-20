using System;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class SubSystemService {
        public long Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public bool IsDeleted { get; set; }

        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }
        public string Wsdl { get; set; }

        public SubSystem SubSystem { get; set; }
        public long SubSystemId { get; set; }
    }
}