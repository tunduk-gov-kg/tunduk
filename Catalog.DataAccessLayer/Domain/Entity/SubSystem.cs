using System;
using System.Collections.Generic;
using Catalog.DataAccessLayer.Domain.Entity.Interfaces;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class SubSystem : IDateTimeTrackable,INonDeletable {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string SubSystemCode { get; set; }
        public Member Member { get; set; }
        public long MemberId { get; set; }
        public List<SubSystemService> Services { get; set; }
    }
}