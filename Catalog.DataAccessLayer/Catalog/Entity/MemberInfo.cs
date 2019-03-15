using System;
using System.Collections.Generic;
using Catalog.DataAccessLayer.XRoad.Entity;

namespace Catalog.DataAccessLayer.Catalog.Entity
{
    public class MemberInfo
    {
        public long MemberInfoId { get; set; }
        public Member Member { get; set; }

        public DateTime? ModificationDateTime { get; set; }

        public string Description { get; set; }
        public string Site { get; set; }

        public MemberStatus MemberStatus { get; set; }
        public long? MemberStatusId { get; set; }

        public MemberType MemberType { get; set; }
        public long? MemberTypeId { get; set; }

        public List<MemberInfoRoleReference> MemberRoleReferences { get; set; }
    }
}