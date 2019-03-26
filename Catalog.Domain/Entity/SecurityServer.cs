namespace Catalog.Domain.Entity {
    public class SecurityServer : BaseEntity, ISoftDelete {
        public bool IsDeleted { get; set; }
        public string SecurityServerCode { get; set; }
        public string Address { get; set; }
        public Member Member { get; set; }
        public long MemberId { get; set; }
    }
}