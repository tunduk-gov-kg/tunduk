namespace Catalog.Domain
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}