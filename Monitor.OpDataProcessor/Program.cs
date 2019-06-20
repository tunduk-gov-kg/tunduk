using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Monitor.Domain;

namespace Monitor.OpDataProcessor
{
    static class Program
    {
        static void Main(string[] args)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var dbContextProvider =
                new DbContextProvider(configurationRoot.GetConnectionString("Monitor"));


            var dataProcessor = new OpDataProcessor(dbContextProvider, new MessagePairMatcher(dbContextProvider));

            for (var processingIndex = 0; processingIndex < 20; processingIndex++)
            {
                Console.WriteLine(
                    $"{processingIndex} iteration started at {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                dataProcessor.ProcessRecords(10_000);

                Console.WriteLine(
                    $"{processingIndex} completed started at {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            }
        }
    }
}