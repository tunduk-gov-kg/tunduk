using XRoad.Domain;

namespace Catalog.Domain.Model
{
    public class ConsumedService
    {
        public string Name { get; set; }
        public ServiceIdentifier Producer { get; set; }
        public RequestsCount RequestsCount { get; set; }
    }
}