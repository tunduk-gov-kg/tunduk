using System;
using System.Text;
using System.Threading.Tasks;
using XRoad.Domain;
using Xunit;

namespace XRoad.GlobalConfiguration.UnitTests
{
    public class ServiceMetadataManagerUnitTests
    {
        [Fact]
        public async Task GetServicesAsync_When__()
        {
            var manager = new ServiceMetadataManager();
            var result = await manager.GetServicesAsync(new Uri("http://10.55.0.4"),
                new SubSystemIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000001",
                    SubSystemCode = "monitoring-system"
                }, new SubSystemIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000005",
                    SubSystemCode = "vehicles-service"
                });

            Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task GetSharedParamsAsync_When__Async()
        {
            var manager = new ServiceMetadataManager();
            var result = await manager.GetSharedParamsAsync(new Uri("http://10.55.0.4"));

            Assert.Equal("central-server", result.InstanceIdentifier);
        }

        [Fact]
        public async Task GetWsdlAsync_When__()
        {
            var manager = new ServiceMetadataManager();
            var result = await manager.GetWsdlAsync(new Uri("http://10.55.0.4"),
                new SubSystemIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000001",
                    SubSystemCode = "monitoring-system"
                }, new ServiceIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000005",
                    SubSystemCode = "zags-service",
                    ServiceCode = "testZagsCatalog",
                    ServiceVersion = "v2"
                });

            var resultWsdl = Encoding.UTF8.GetString(result);
            Assert.True(result.Length > 0);
        }
    }
}