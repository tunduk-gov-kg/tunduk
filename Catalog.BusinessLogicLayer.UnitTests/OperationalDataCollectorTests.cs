using System;
using AutoMapper;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.UnitTests.Providers;
using Catalog.Domain.Mapping;
using Microsoft.Extensions.Logging;
using XRoad.Domain;
using XRoad.OpMonitor;
using Xunit;
using Xunit.Abstractions;
using XUnit.Helpers;

namespace Catalog.BusinessLogicLayer.UnitTests
{
    public class OperationalDataCollectorTests
    {
        private readonly ILogger<OperationalDataCollector> _logger;

        public OperationalDataCollectorTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XUnitLoggerProvider(testOutputHelper));
            _logger = loggerFactory.CreateLogger<OperationalDataCollector>();
        }

        [Fact]
        public void RunOpDataCollectorTask__When__()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(typeof(SecurityServerProfile));
                cfg.AddProfiles(typeof(OperationalDataRecordProfile));
            });

            var monitoringService = new OperationalDataCollector(DbContextProvider.RequireDbContext(),
                mapperConfiguration.CreateMapper(), new OperationalDataService(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _logger);

            monitoringService.RunOpDataCollectorTask();
        }

        [Fact]
        public void RunOpDataCollectorTask__When__SecuirtyServer()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(typeof(SecurityServerProfile));
                cfg.AddProfiles(typeof(OperationalDataRecordProfile));
            });

            var monitoringService = new OperationalDataCollector(
                DbContextProvider.RequireDbContext(),
                mapperConfiguration.CreateMapper(),
                new OperationalDataService(),
                XRoadExchangeParametersProvider.RequireXRoadExchangeParameters(),
                _logger
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