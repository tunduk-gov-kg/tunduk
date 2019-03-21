using System;
using System.Collections.Generic;
using Catalog.DataAccessLayer.Domain.Entity.Interfaces;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class MemberInfo : IUserTrackable<string>, IDateTimeTrackable {
        public string ModifierId { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long MemberInfoId { get; set; }
        public Member Member { get; set; }
        public string Description { get; set; }
        public string Site { get; set; }
        public MemberStatus MemberStatus { get; set; }
        public long? MemberStatusId { get; set; }
        public MemberType MemberType { get; set; }
        public long? MemberTypeId { get; set; }
        public List<MemberInfoRoleReference> MemberRoleReferences { get; set; }
    }
}