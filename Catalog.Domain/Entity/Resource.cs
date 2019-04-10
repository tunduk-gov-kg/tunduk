namespace Catalog.Domain.Entity
{
    public class Resource : UserTrackableEntity
    {
        public byte[] Content { get; set; }
        public string Name { get; set; }
        public string MediaType { get; set; }
    }
}