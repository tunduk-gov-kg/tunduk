using System.Collections.Generic;

namespace Catalog.DataAccessLayer.Domain.Entity {
    public class MemberType {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<MemberInfo> MemberInfoRecords { get; set; }
    }
}