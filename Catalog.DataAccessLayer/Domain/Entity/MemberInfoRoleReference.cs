namespace Catalog.DataAccessLayer.Domain.Entity {
    public class MemberInfoRoleReference {
        public MemberInfo MemberInfo { get; set; }
        public MemberRole MemberRole { get; set; }

        public long MemberInfoId { get; set; }
        public long MemberRoleId { get; set; }
    }
}