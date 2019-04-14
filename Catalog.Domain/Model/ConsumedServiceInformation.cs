using XRoad.Domain;

namespace Catalog.Domain.Model
{
    public class ConsumedServiceInformation
    {
        public SubSystemIdentifier Consumer { get; set; }
        public ServiceIdentifier Producer { get; set; }
        public RequestsCount RequestsCount { get; set; }
    }
}