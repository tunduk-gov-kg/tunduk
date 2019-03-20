using System;
using System.Collections.Generic;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class SubSystem {
        public long Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public bool IsDeleted { get; set; }

        public string SubSystemCode { get; set; }

        public Member Member { get; set; }
        public long MemberId { get; set; }

        public List<SubSystemService> Services { get; set; }
    }
}