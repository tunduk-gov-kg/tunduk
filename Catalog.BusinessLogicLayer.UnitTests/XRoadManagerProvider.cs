using System;
using Catalog.BusinessLogicLayer.Service;
using XRoad.Domain;
using XRoad.GlobalConfiguration;

namespace Catalog.BusinessLogicLayer.UnitTests {
    public static class XRoadManagerProvider {
        public static XRoadManager RequireXRoadManager() {
            return new XRoadManager(new ServiceMetadataManager(), new XRoadExchangeParameters {
                ClientSubSystem = new SubSystemIdentifier {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000001",
                    SubSystemCode = "monitoring-system"
                },
                SecurityServerUri = new Uri("http://10.55.0.4")
            }, new MockExceptionHandler());
        }
    }
}