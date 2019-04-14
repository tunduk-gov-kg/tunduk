using System;
using System.Collections.Generic;

namespace Catalog.Domain.Model
{
    public class MemberExchangeInformation
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        
        public List<SubSystemExchangeInformation> SubSystems { get; set; }
        public ExchangeInformation ExchangeInformation { get; set; }
        
        public RequestsCount TotalIncomingRequestsCount { get; set; }
        public RequestsCount TotalOutgoingRequestsCount { get; set; }
    }
}