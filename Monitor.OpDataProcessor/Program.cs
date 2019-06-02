using System.IO;
using Microsoft.Extensions.Configuration;
using Monitor.Domain;
using Monitor.Domain.Repository;

namespace Monitor.OpDataProcessor
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextProvider =
                new DbContextProvider(configurationRoot.GetConnectionString("Monitor"));

            var dataRepository = new OpDataRepository(dbContextProvider);

            var dataProcessor = new OpDataProcessor(dbContextProvider, new MessagePairMatcher(dbContextProvider),
                dataRepository);

            dataProcessor.ProcessRecords();
        }
    }
}