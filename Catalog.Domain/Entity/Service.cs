namespace Catalog.Domain.Entity
{
    public class Service : UserTrackableEntity, ISoftDelete
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }
        public string Wsdl { get; set; }
        public SubSystem SubSystem { get; set; }
        public long SubSystemId { get; set; }
        public bool IsDeleted { get; set; }
    }
}