using System;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.BusinessLogicLayer.Service;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.Daemon.HostedService;
using Catalog.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using XRoad.Domain;
using XRoad.GlobalConfiguration;
using XRoad.OpMonitor;

namespace Catalog.Daemon
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureLogging((hostContext, config) =>
                {
                    config.AddConsole();
                    config.AddDebug();
                    config.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                })
                .ConfigureHostConfiguration(config => { config.AddEnvironmentVariables(); })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        true);
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddAutoMapper();
                    services.AddScoped<IOperationalDataService, OperationalDataService>();
                    services.AddSingleton(provider => new XRoadExchangeParameters
                    {
                        SecurityServerUri = new Uri(hostContext.Configuration["SecurityServerUri"]),
                        ClientSubSystem = new SubSystemIdentifier
                        {
                            Instance = hostContext.Configuration["Instance"],
                            MemberClass = hostContext.Configuration["MemberClass"],
                            MemberCode = hostContext.Configuration["MemberCode"],
                            SubSystemCode = hostContext.Configuration["SubSystemCode"]
                        }
                    });

                    services.AddScoped<IServiceMetadataManager, ServiceMetadataManager>();
                    services.AddScoped<IXRoadStorageUpdater<MemberData>, MembersStorageUpdater>();
                    services.AddScoped<IXRoadStorageUpdater<SecurityServerData>, SecurityServersStorageUpdater>();
                    services.AddScoped<IXRoadStorageUpdater<SubSystemIdentifier>, SubSystemsStorageUpdater>();
                    services.AddScoped<ServicesStorageUpdater>();

                    services.AddScoped<IXRoadGlobalConfigurationClient, XRoadGlobalConfigurationClient>();

                    services.AddDbContext<CatalogDbContext>(builder =>
                        builder.UseNpgsql(hostContext.Configuration.GetConnectionString("CatalogDb")));

                    services.AddScoped<XRoadOperationalDataCollector>();
                    services.AddScoped<XRoadOperationalDataProcessor>();
                    services.AddScoped<XRoadMetadataCollector>();

                    services.AddHostedService<OperationalDataCollectorService>();
                    services.AddHostedService<OperationalDataProcessorService>();
                    services.AddHostedService<XRoadMetadataCollectorService>();
                })
                .UseConsoleLifetime()
                .Build();

            using (host)
            {
                await host.StartAsync();
                await host.WaitForShutdownAsync();
            }
        }
    }
}