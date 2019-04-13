using System.Collections.Generic;
using XRoad.Domain;

namespace Catalog.Domain.Model
{
    public class ProducedServiceInformation
    {
        public ServiceIdentifier ServiceIdentifier { get; set; }
        public List<ConsumedServiceInformation> Consumers { get; set; }

        public RequestsCount RequestsCount { get; set; }
    }
}