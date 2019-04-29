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
        public XRoadOperationalDataCollectorTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XUnitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<XRoadOperationalDataCollector>();
        }

        private readonly ILogger<XRoadOperationalDataCollector> _logger;

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
                _logger,
                dbContextOptionsBuilder.Options
            );

            monitoringService.RunOpDataCollectorTask();
        }

        [Fact]
        public void RunOpDataCollectorTask__When__SecuirtyServer()
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
                _logger,
                dbContextOptionsBuilder.Options
            );

            var nextRecordsFrom = monitoringService.RunOpDataCollectorTask(
                new SecurityServerIdentifier
                {
                    Instance = "KG",
                    MemberClass = "GOV",
                    MemberCode = "70000001",
                    SecurityServerCode = "MANAGEMENT-SERVER"
                },
                new DateTime(2019, 4, 10)
            );
        }
    }
}