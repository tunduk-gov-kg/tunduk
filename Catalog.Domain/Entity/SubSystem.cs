using System.Collections.Generic;

namespace Catalog.Domain.Entity
{
    public class SubSystem : UserTrackableEntity, ISoftDelete
    {
        public string SubSystemCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Member Member { get; set; }
        public long MemberId { get; set; }
        public List<Service> Services { get; set; }
        public string NormalizedName => Name ?? SubSystemCode;
        public bool IsDeleted { get; set; }
    }
}