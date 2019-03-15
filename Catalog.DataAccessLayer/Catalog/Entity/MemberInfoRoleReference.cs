namespace Catalog.DataAccessLayer.Catalog.Entity
{
    public class MemberInfoRoleReference
    {
        public long Id { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public MemberRole MemberRole { get; set; }

        public long MemberInfoId { get; set; }
        public long MemberRoleId { get; set; }
    }
}