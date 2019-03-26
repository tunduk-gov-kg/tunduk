namespace Catalog.Domain {
    public abstract class UserTrackableEntity : BaseEntity {
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}