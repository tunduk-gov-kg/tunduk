using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.UnitTests.Providers;
using Microsoft.Extensions.Logging;
using XRoad.GlobalConfiguration;
using Xunit;
using Xunit.Abstractions;
using XUnit.Helpers;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class XRoadGlobalConfigurationClientTests
    {
        private readonly ILogger<XRoadGlobalConfigurationClient> _xRoadManageLogger;

        public XRoadGlobalConfigurationClientTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XUnitLoggerProvider(testOutputHelper));
            _xRoadManageLogger = loggerFactory.CreateLogger<XRoadGlobalConfigurationClient>();
        }

        [Fact]
        public async Task GetMembersListAsync_When__()
        {
            var manager = new XRoadGlobalConfigurationClient(
                new ServiceMetadataManager(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _xRoadManageLogger
            );

            var immutableList = await manager.GetMembersListAsync();
            Assert.True(immutableList.Count > 0);
        }

        [Fact]
        public async Task GetSecurityServersListAsync_When__()
        {
            var manager = new XRoadGlobalConfigurationClient(
                new ServiceMetadataManager(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _xRoadManageLogger
            );
            var immutableList = await manager.GetSecurityServersListAsync();
            Assert.True(immutableList.Count > 0);
        }

        [Fact]
        public async Task GetServicesListAsync_When__()
        {
            var manager = new XRoadGlobalConfigurationClient(
                new ServiceMetadataManager(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _xRoadManageLogger
            );
            var servicesListAsync = await manager.GetServicesListAsync();
            Assert.True(servicesListAsync.Count > 0);
        }

        [Fact]
        public async Task GetSubSystemsListAsync_When__()
        {
            var manager = new XRoadGlobalConfigurationClient(
                new ServiceMetadataManager(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _xRoadManageLogger
            );
            var immutableList = await manager.GetSubSystemsListAsync();
            Assert.True(immutableList.Count > 0);
        }
    }
}