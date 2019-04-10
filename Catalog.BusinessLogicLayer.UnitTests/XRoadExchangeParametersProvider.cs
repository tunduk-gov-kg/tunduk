using System;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public static class XRoadExchangeParametersProvider
    {
        public static XRoadExchangeParameters RequireXRoadExchangeParameters()
        {
            return new XRoadExchangeParameters
            {
                ClientSubSystem = new SubSystemIdentifier
                {
                    Instance = "KG",
                    MemberClass = "GOV",
                    MemberCode = "70000001",
                    SubSystemCode = "MONITORING-SUBSYSTEM"
                },
                SecurityServerUri = new Uri("http://10.94.7.70")
            };
        }
    }
}