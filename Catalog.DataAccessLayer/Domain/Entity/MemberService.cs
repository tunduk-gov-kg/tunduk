using System;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class MemberService {
        public long Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public bool IsDeleted { get; set; }

        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }
        public string Wsdl { get; set; }

        public Member Member { get; set; }
        public long MemberId { get; set; }
    }
}