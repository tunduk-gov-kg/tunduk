using System;

namespace Catalog.DataAccessLayer.XRoad.Entity
{
    public class SecurityServer
    {
        public long Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public bool IsDeleted { get; set; }

        public string SecurityServerCode { get; set; }
        public string Address { get; set; }

        public Member Member { get; set; }
        public long MemberId { get; set; }
    }
}
