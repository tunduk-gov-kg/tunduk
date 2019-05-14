using System;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.UnitTests.Providers
{
    public static class XRoadExchangeParametersProvider
    {
        public static XRoadExchangeParameters RequireXRoadExchangeParameters()
        {
            return new XRoadExchangeParameters
            {
                ClientSubSystem = new SubSystemIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000001",
                    SubSystemCode = "monitoring-system"
                },
                SecurityServerUri = new Uri("http://10.55.0.4")
            };
        }
    }
}