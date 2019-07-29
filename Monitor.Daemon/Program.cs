using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monitor.Daemon.Extensions;
using Monitor.Daemon.Service;
using Monitor.Domain;
using Npgsql;
using XRoad.Domain;
using XRoad.GlobalConfiguration;
using XRoad.OpMonitor;

namespace Monitor.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configurationRoot.GetConnectionString("Monitor");

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<MonitorDbContext>(options => { options.UseNpgsql(connectionString); });
            serviceCollection.AddScoped<IServerManager, ServerManager>();
            serviceCollection.AddScoped<IOpDataStorage, OpDataStorage>();

            serviceCollection.AddSingleton<IConfiguration>(configurationRoot);

            serviceCollection.AddLogging(builder => { builder.AddConsole(); });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            DbMigration(connectionString);

            var exchangeParameters = new XRoadExchangeParameters
            {
                SecurityServerUri = new Uri(configurationRoot["SecurityServerUri"]),
                ClientSubSystem = new SubSystemIdentifier
                {
                    Instance = configurationRoot["Instance"],
                    MemberClass = configurationRoot["MemberClass"],
                    MemberCode = configurationRoot["MemberCode"],
                    SubSystemCode = configurationRoot["SubSystemCode"]
                }
            };

            UpdateServersList(serviceProvider, exchangeParameters);

            var opDataCollector =
                new OpDataCollector(serviceProvider, exchangeParameters, new OperationalDataService());
            opDataCollector.RunCollector();
        }

        private static void DbMigration(string connectionString)
        {
            var cnx = new NpgsqlConnection(connectionString);
            var evolve = new Evolve.Evolve(cnx, Console.WriteLine)
            {
                Locations = new[] {"SQL_Scripts"},
                IsEraseDisabled = true
            };
            evolve.Migrate();
        }

        private static void UpdateServersList(IServiceProvider serviceProvider,
            XRoadExchangeParameters xRoadExchangeParameters)
        {
            var serviceScope = serviceProvider.CreateScope();
            using (serviceScope)
            {
                var serverManager = serviceScope.ServiceProvider.GetService<IServerManager>();
                var serviceMetadataManager = new ServiceMetadataManager();
                var serversListAsync =
                    serviceMetadataManager.GetSecurityServersListAsync(xRoadExchangeParameters.SecurityServerUri);
                var sharedParams = serversListAsync.Result;
                serverManager.UpdateServersList(sharedParams);
            }
        }
    }
}