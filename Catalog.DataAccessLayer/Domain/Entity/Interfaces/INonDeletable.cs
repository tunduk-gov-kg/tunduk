namespace Catalog.DataAccessLayer.Domain.Entity.Interfaces {
    public interface INonDeletable {
        bool IsDeleted { get; set; }
    }
}