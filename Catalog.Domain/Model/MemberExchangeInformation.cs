using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Domain.Model
{
    [Serializable]
    public class MemberExchangeInformation
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<SubSystemExchangeInformation> SubSystems { get; set; }
        public ExchangeInformation ExchangeInformation { get; set; }

        public RequestsCount TotalIncomingRequestsCount
        {
            get
            {
                return new RequestsCount(
                    ExchangeInformation.IncomingRequestsCount.Failed +
                    SubSystems.Sum(it => it.ExchangeInformation.IncomingRequestsCount.Failed),
                    ExchangeInformation.IncomingRequestsCount.Succeeded +
                    SubSystems.Sum(it => it.ExchangeInformation.IncomingRequestsCount.Succeeded)
                );
            }
        }

        public RequestsCount TotalOutgoingRequestsCount
        {
            get
            {
                return new RequestsCount(
                    ExchangeInformation.OutgoingRequestsCount.Failed +
                    SubSystems.Sum(it => it.ExchangeInformation.OutgoingRequestsCount.Failed),
                    ExchangeInformation.OutgoingRequestsCount.Succeeded +
                    SubSystems.Sum(it => it.ExchangeInformation.OutgoingRequestsCount.Succeeded)
                );
            }
        }
    }
}