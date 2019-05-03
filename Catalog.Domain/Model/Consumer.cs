using XRoad.Domain;

namespace Catalog.Domain.Model
{
    public class Consumer
    {
        public string Name { get; set; }
        public SubSystemIdentifier ConsumerIdentifier { get; set; }
        public RequestsCount RequestsCount { get; set; }
    }
}