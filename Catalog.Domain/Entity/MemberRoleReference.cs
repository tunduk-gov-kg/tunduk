using Catalog.Domain.Enum;

namespace Catalog.Domain.Entity
{
    public class MemberRoleReference
    {
        public MemberRole MemberRole { get; set; }
        public Member Member { get; set; }
        public long MemberId { get; set; }
    }
}