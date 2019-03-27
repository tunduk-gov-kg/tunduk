using System.Collections.Generic;
using Catalog.Domain.Enum;

namespace Catalog.Domain.Entity {
    public class Member : UserTrackableEntity, ISoftDelete {
        public string Instance { get; set; }
        public string MemberClass { get; set; }
        public string MemberCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public MemberStatus MemberStatus { get; set; }
        public MemberType MemberType { get; set; }
        public List<MemberRoleReference> MemberRoles { get; set; }
        public List<SubSystem> SubSystems { get; set; }
        public List<SecurityServer> SecurityServers { get; set; }
        public bool IsDeleted { get; set; }
    }
}