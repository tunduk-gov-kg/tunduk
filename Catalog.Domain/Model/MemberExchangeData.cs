using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Domain.Model
{
    [Serializable]
    public class MemberExchangeData
    {
        public List<ExchangeData> SubSystems { get; set; }
        public ExchangeData ExchangeData { get; set; }

        public RequestsCount TotalIncomingRequestsCount
        {
            get
            {
                return new RequestsCount(
                    ExchangeData.IncomingRequestsCount.Failed +
                    SubSystems.Sum(it => it.IncomingRequestsCount.Failed),
                    ExchangeData.IncomingRequestsCount.Succeeded +
                    SubSystems.Sum(it => it.IncomingRequestsCount.Succeeded)
                );
            }
        }

        public RequestsCount TotalOutgoingRequestsCount
        {
            get
            {
                return new RequestsCount(
                    ExchangeData.OutgoingRequestsCount.Failed +
                    SubSystems.Sum(it => it.OutgoingRequestsCount.Failed),
                    ExchangeData.OutgoingRequestsCount.Succeeded +
                    SubSystems.Sum(it => it.OutgoingRequestsCount.Succeeded)
                );
            }
        }
    }
}