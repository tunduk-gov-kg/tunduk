using System.Collections.Generic;
using System.Linq;
using XRoad.Domain;

namespace Catalog.Domain.Model
{
    public class ExchangeData
    {
        public string Name { get; set; }
        public SubSystemIdentifier Identifier { get; set; }
        public List<ConsumedService> ConsumedServices { get; set; }
        public List<ProducedService> ProducedServices { get; set; }

        public RequestsCount OutgoingRequestsCount
        {
            get
            {
                return new RequestsCount(
                    ConsumedServices.Sum(it => it.RequestsCount.Failed),
                    ConsumedServices.Sum(it => it.RequestsCount.Succeeded)
                );
            }
        }

        public RequestsCount IncomingRequestsCount
        {
            get
            {
                return new RequestsCount(
                    ProducedServices.Sum(it => it.RequestsCount.Failed),
                    ProducedServices.Sum(it => it.RequestsCount.Succeeded)
                );
            }
        }
    }
}