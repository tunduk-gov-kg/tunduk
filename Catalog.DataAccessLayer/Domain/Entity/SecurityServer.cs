using System;
using Catalog.DataAccessLayer.Domain.Entity.Interfaces;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class SecurityServer : INonDeletable, IDateTimeTrackable {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public string SecurityServerCode { get; set; }
        public string Address { get; set; }
        public Member Member { get; set; }
        public long MemberId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}