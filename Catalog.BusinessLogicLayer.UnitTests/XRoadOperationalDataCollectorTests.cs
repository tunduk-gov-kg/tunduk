using System;
using AutoMapper;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.UnitTests.Providers;
using Catalog.DataAccessLayer;
using Catalog.Domain.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using XRoad.Domain;
using XRoad.OpMonitor;
using Xunit;
using Xunit.Abstractions;
using XUnit.Helpers;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class XRoadOperationalDataCollectorTests
    {
        private ITestOutputHelper _testOutputHelper;
        public XRoadOperationalDataCollectorTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XUnitLoggerProvider(testOutputHelper));
            _opDataCollectorLogger = loggerFactory.CreateLogger<XRoadOperationalDataCollector>();
            _repositoryLogger = loggerFactory.CreateLogger<OperationalDataRepository>();
        }

        private readonly ILogger<XRoadOperationalDataCollector> _opDataCollectorLogger;
        private readonly ILogger<OperationalDataRepository> _repositoryLogger;

        [Fact]
        public void RunOpDataCollectorTask__When__()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=Tunduk;User Id=postgres;Password=postgres;");

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(typeof(SecurityServerProfile));
                cfg.AddProfiles(typeof(OperationalDataRecordProfile));
            });

            var monitoringService = new XRoadOperationalDataCollector(
                mapperConfiguration.CreateMapper(),
                new OperationalDataService(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _opDataCollectorLogger,
                dbContextOptionsBuilder.Options,
                new OperationalDataRepository(dbContextOptionsBuilder.Options,_repositoryLogger)
            );

            monitoringService.RunOpDataCollectorTask();
        }

        [Fact]
        public void RunOpDataCollectorTask__When__SecurityServer()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=Tunduk;User Id=postgres;Password=postgres;");

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(typeof(SecurityServerProfile));
                cfg.AddProfiles(typeof(OperationalDataRecordProfile));
            });

            var monitoringService = new XRoadOperationalDataCollector(
                mapperConfiguration.CreateMapper(),
                new OperationalDataService(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _opDataCollectorLogger,
                dbContextOptionsBuilder.Options,
                new OperationalDataRepository(dbContextOptionsBuilder.Options,_repositoryLogger)
            );

            var nextRecordsFrom = monitoringService.RunOpDataCollectorTask(
                new SecurityServerIdentifier
                {
                    Instance = "central-server",
                    MemberClass = "GOV",
                    MemberCode = "70000005",
                    SecurityServerCode = "grs-security-server"
                },
                new DateTime(2019, 5, 13)
            );
            _testOutputHelper.WriteLine(nextRecordsFrom.ToString());
        }
    }
}