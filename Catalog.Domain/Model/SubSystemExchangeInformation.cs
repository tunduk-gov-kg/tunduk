using System.Collections.Generic;
using XRoad.Domain;

namespace Catalog.Domain.Model
{
    public class SubSystemExchangeInformation
    {
        public SubSystemIdentifier SubSystemIdentifier { get; set; }
        public List<ConsumedServiceInformation> ConsumedServices { get; set; }
        public List<ProducedServiceInformation> ProducedServices { get; set; }

        public RequestsCount IncomingRequestsCount { get; set; }
        public RequestsCount OutgoingRequestsCount { get; set; }
    }
}