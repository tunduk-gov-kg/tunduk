using System;
using System.Collections.Generic;
using Catalog.DataAccessLayer.Domain.Entity.Interfaces;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class Member : IDateTimeTrackable, INonDeletable {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string Instance { get; set; }
        public string MemberClass { get; set; }
        public string MemberCode { get; set; }
        public string Name { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public List<SubSystem> SubSystems { get; set; }
        public List<SecurityServer> SecurityServers { get; set; }
        public List<MemberService> Services { get; set; }
    }
}