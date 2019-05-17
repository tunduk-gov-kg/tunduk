using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Monitor.Migrations
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var configurationRoot = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var cnx = new NpgsqlConnection(configurationRoot.GetConnectionString("Monitor"));
                var evolve = new Evolve.Evolve(cnx, Console.WriteLine)
                {
                    Locations = new[] {"SQL_Scripts"},
                    IsEraseDisabled = true,
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}