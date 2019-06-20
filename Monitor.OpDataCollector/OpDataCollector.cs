using System;
using System.Collections.Generic;
using System.Linq;
using Monitor.Domain;
using Monitor.Domain.Entity;
using Monitor.Domain.Extensions;
using Monitor.OpDataCollector.Extensions;
using SimpleSOAPClient.Exceptions;
using XRoad.Domain;
using XRoad.OpMonitor;
using XRoad.OpMonitor.Domain;
using XRoad.OpMonitor.Domain.SOAP;

namespace Monitor.OpDataCollector
{
    public class OpDataCollector
    {
        private readonly XRoadExchangeParameters _exchangeParameters;
        private readonly IOperationalDataService _opDataReader;
        private readonly IDbContextProvider _dbContextProvider;

        public OpDataCollector(IOperationalDataService opDataReader
            , XRoadExchangeParameters exchangeParameters
            , IDbContextProvider dbContextProvider)
        {
            _opDataReader = opDataReader;
            _exchangeParameters = exchangeParameters;
            _dbContextProvider = dbContextProvider;
        }

        public void Collect()
        {
            List<Server> servers;

            using (var dbContext = _dbContextProvider.CreateDbContext())
                servers = dbContext.Servers.ToList();

            servers.AsParallel().ForAll(server =>
            {
                try
                {
                    Collect(server);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine(exception.StackTrace);
                }
            });
        }

        private void Collect(Server server)
        {
            const int offsetSeconds = 240;
            const int maxIteration = 10;
            
            // security servers use utc time 
            var recordsTo = DateTime.UtcNow.ToSeconds() - offsetSeconds;
            
            for (var i = 0; i < maxIteration; i++)
            {
                Console.WriteLine(
                    $"Requesting logs from: {server.GetIdentifier()} between {server.NextRecordsFromTimestamp} to {recordsTo}");

                if (TryGetOpData(out var operationalData, server.GetIdentifier(), server.NextRecordsFromTimestamp,
                    recordsTo))
                {
                    Console.WriteLine(
                        $"Fetched logs {operationalData.Records.Length} from: {server.GetIdentifier()} between {server.NextRecordsFromTimestamp} to {recordsTo}");

                    var dataRecords = Array.ConvertAll(operationalData.Records, OpDataRecordExtensions.Convert);
                    
                    ProcessOpDataRecords(server, dataRecords, operationalData, recordsTo);
                    
                    if (!operationalData.NextRecordsFromSpecified)
                    {
                        break;
                    }
                    
                    Console.WriteLine($"{server.GetIdentifier()} - next records from equals: {operationalData.NextRecordsFrom}");
                }
                else break;
            }
        }

        private void ProcessOpDataRecords(Server server, OpDataRecord[] dataRecords, OperationalData operationalData,
            long recordsTo)
        {
            using (var dbContext = _dbContextProvider.CreateDbContext())
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                if (!operationalData.NextRecordsFromSpecified)
                {
                    server.NextRecordsFromTimestamp = recordsTo + 1;
                }
                else
                {
                    // ReSharper disable once PossibleInvalidOperationException
                    server.NextRecordsFromTimestamp = operationalData.NextRecordsFrom.Value;
                }

                dbContext.OpDataRecords.AddRange(dataRecords);
                dbContext.Servers.Update(server);
                dbContext.SaveChanges();
                transaction.Commit();
            }
        }


        /// <summary>
        /// Retrieves operational data from given security server
        /// </summary>
        /// <param name="operationalData">out parameter to init with op data records</param>
        /// <param name="securityServerIdentifier">target security server identifier</param>
        /// <param name="recordsFrom">The beginning of the time interval of requested operational data (Unix timestamp in seconds)</param>
        /// <param name="recordsTo">The end of the time interval of requested operational data (Unix timestamp in seconds)</param>
        /// <returns></returns>
        private bool TryGetOpData(out OperationalData operationalData,
            SecurityServerIdentifier securityServerIdentifier, long recordsFrom, long recordsTo)
        {
            try
            {
                var searchCriteria = new SearchCriteria
                {
                    RecordsFrom = recordsFrom,
                    RecordsTo = recordsTo
                };
                operationalData =
                    _opDataReader.GetOperationalData(_exchangeParameters, securityServerIdentifier, searchCriteria);
                return true;
            }
            catch (FaultException exception)
            {
                Console.WriteLine($"{securityServerIdentifier}: " + exception.String);
                operationalData = null;
                return false;
            }
        }
    }
}