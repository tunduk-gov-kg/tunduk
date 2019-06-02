using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Monitor.Domain;
using Monitor.Domain.Repository;
using XRoad.Domain;
using XRoad.GlobalConfiguration;
using XRoad.OpMonitor;

namespace Monitor.OpDataCollector
{
    internal class Program
    {
        private static async Task Main(string[] args)
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

            IServerRepository serverRepository = new ServerRepository(dbContextProvider);
            IOpDataRepository opDataRepository = new OpDataRepository(dbContextProvider);
            var serversProvider = new ServersProvider(new ServiceMetadataManager());

            var servers = await serversProvider.GetSecurityServersListAsync(exchangeParameters.SecurityServerUri);
            serverRepository.UpdateLocalCache(servers);

            var opDataCollector = new OpDataCollector(serverRepository, new OperationalDataService(),
                exchangeParameters, opDataRepository);
            opDataCollector.Collect();
        }
    }
}