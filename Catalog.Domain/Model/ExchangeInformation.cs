using System.Collections.Generic;
using System.Linq;

namespace Catalog.Domain.Model
{
    public class ExchangeInformation
    {
        public List<ConsumedServiceInformation> ConsumedServices { get; set; }
        public List<ProducedServiceInformation> ProducedServices { get; set; }

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