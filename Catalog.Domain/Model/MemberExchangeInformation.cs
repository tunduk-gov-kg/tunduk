using System.Collections.Generic;

namespace Catalog.Domain.Model
{
    public class MemberExchangeInformation
    {
        public List<SubSystemExchangeInformation> SubSystems { get; set; }
        public List<ConsumedServiceInformation> ConsumedServices { get; set; }
        public List<ProducedServiceInformation> ProducedServices { get; set; }

        public RequestsCount OutoingRequestsCount { get; set; }
        public RequestsCount IncomingRequestsCount { get; set; }
    }
}