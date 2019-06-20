using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Monitor.Domain;
using Monitor.OpDataCollector.Extensions;
using XRoad.Domain;
using XRoad.GlobalConfiguration;
using XRoad.OpMonitor;

namespace Monitor.OpDataCollector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var dbContextProvider =
                new DbContextProvider(configurationRoot.GetConnectionString("Monitor"));

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
            
            Console.WriteLine($"OpDataCollector task starting with args: {exchangeParameters.SecurityServerUri}; {exchangeParameters.ClientSubSystem}");

            var serversProvider = new ServersProvider(new ServiceMetadataManager());
            var servers = await serversProvider.GetSecurityServersListAsync(exchangeParameters.SecurityServerUri);
            
            var dbContext = dbContextProvider.CreateDbContext();
            dbContext.UpdateServersList(servers);
            dbContext.Dispose();
            
            var opDataCollector = new OpDataCollector(new OperationalDataService(), exchangeParameters,dbContextProvider);
            
            opDataCollector.Collect();
        }
    }
}