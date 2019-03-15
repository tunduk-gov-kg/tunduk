using System.Collections.Generic;

namespace Catalog.DataAccessLayer.Catalog.Entity
{
    public class MemberStatus
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<MemberInfo> MemberInfoRecords { get; }
    }
}