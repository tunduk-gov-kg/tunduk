using System.Collections.Generic;
using System.Linq;
using XRoad.Domain;

namespace Catalog.Domain.Model
{
    public class ProducedServiceInformation
    {
        public ServiceIdentifier ServiceIdentifier { get; set; }
        public List<ConsumedServiceInformation> Consumers { get; set; }

        public RequestsCount RequestsCount
        {
            get
            {
                return new RequestsCount(
                    Consumers.Sum(it => it.RequestsCount.Failed),
                    Consumers.Sum(it => it.RequestsCount.Succeeded)
                );
            }
        }
    }
}