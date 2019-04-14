using System.Collections.Generic;

namespace Catalog.Domain.Model
{
    public class ExchangeInformation
    {
        public List<ConsumedServiceInformation> ConsumedServices { get; set; }
        public List<ProducedServiceInformation> ProducedServices { get; set; }
        
        public RequestsCount OutgoingRequestsCount { get; set; }
        public RequestsCount IncomingRequestsCount { get; set; }
    }
}