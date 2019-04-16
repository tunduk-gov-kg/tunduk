using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.UnitTests.Providers;
using Microsoft.Extensions.Logging;
using XRoad.Domain;
using XRoad.GlobalConfiguration;
using Xunit;
using Xunit.Abstractions;
using XUnit.Helpers;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class XRoadMetadataCollectorTests
    {
        private readonly ILogger<XRoadMetadataCollector> _logger;
        private readonly ILogger<XRoadGlobalConfigurationClient> _xRoadManageLogger;

        public XRoadMetadataCollectorTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XUnitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<XRoadMetadataCollector>();
            _xRoadManageLogger = loggerFactory.CreateLogger<XRoadGlobalConfigurationClient>();
        }

        [Fact]
        public async Task RunBatchUpdateTask__When__()
        {
            var manager = new XRoadGlobalConfigurationClient(
                new ServiceMetadataManager(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _xRoadManageLogger
            );

            var defaultUpdateManager = new XRoadMetadataCollector(
                manager,
                new MembersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SecurityServersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SubSystemsStorageUpdater(DbContextProvider.RequireDbContext()),
                new ServicesStorageUpdater(DbContextProvider.RequireDbContext()),
                _logger
            );
            await defaultUpdateManager.RunBatchUpdateTask();
        }

        [Fact]
        public async Task RunWsdlUpdateTaskAsync__When__()
        {
            var manager = new XRoadGlobalConfigurationClient(
                new ServiceMetadataManager(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _xRoadManageLogger
            );

            var defaultUpdateManager = new XRoadMetadataCollector(
                manager,
                new MembersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SecurityServersStorageUpdater(DbContextProvider.RequireDbContext()),
                new SubSystemsStorageUpdater(DbContextProvider.RequireDbContext()),
                new ServicesStorageUpdater(DbContextProvider.RequireDbContext()),
                _logger
            );

            await defaultUpdateManager.RunWsdlUpdateTask(
                new ServiceIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000003",
                    SubSystemCode = "settlements-service",
                    ServiceCode = "GetWorkPeriodInfoWithSum"
                });
        }
    }
}