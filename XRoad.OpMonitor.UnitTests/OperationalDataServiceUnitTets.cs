using System;
using System.Threading.Tasks;
using XRoad.Domain;
using XRoad.OpMonitor.Domain.SOAP;
using Xunit;

namespace XRoad.OpMonitor.UnitTests
{
    public class OperationalDataServiceUnitTets
    {
        [Fact]
        public async Task GetOperationalDataAsync__When()
        {
            var service = new OperationalDataService();
            var operationalData = await service.GetOperationalDataAsync(
                new XRoadExchangeParameters
                {
                    ClientSubSystem = new SubSystemIdentifier
                    {
                        Instance = "central-server",
                        MemberClass = "GOV",
                        MemberCode = "70000001",
                        SubSystemCode = "monitoring-system"
                    },
                    SecurityServerUri = new Uri("http://10.55.0.4")
                }, new SecurityServerIdentifier
                {
                    Instance = "central-server",
                    MemberCode = "70000001",
                    MemberClass = "GOV",
                    SecurityServerCode = "management-server"
                }, new SearchCriteria
                {
                    RecordsTo = 1554789177,
                    RecordsFrom = 0L
                });
            Assert.True(operationalData.Records.Length > 0);
        }
    }
}