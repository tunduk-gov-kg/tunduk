using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monitor.Daemon.Extensions;
using Monitor.Domain.Entity;
using Monitor.Domain.Extensions;
using SimpleSOAPClient.Exceptions;
using XRoad.Domain;
using XRoad.OpMonitor;
using XRoad.OpMonitor.Domain;
using XRoad.OpMonitor.Domain.SOAP;

namespace Monitor.Daemon.Service
{
    public class OpDataCollector
    {
        private readonly ILogger<OpDataCollector> _logger;

        private readonly IServiceProvider _serviceProvider;
        private readonly XRoadExchangeParameters _exchangeParameters;
        private readonly IOperationalDataService _opDataReader;

        public OpDataCollector(IServiceProvider serviceProvider,
            XRoadExchangeParameters exchangeParameters,
            IOperationalDataService opDataReader)
        {
            _serviceProvider = serviceProvider;
            _exchangeParameters = exchangeParameters;
            _opDataReader = opDataReader;
            _logger = _serviceProvider.GetService<ILogger<OpDataCollector>>();
        }

        public void RunCollector()
        {
            var serversList = GetServersList();
            serversList.AsParallel().ForAll(RunCollector);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public void RunCollector(Server server)
        {
            try
            {
                const int offsetSeconds = 240;
                const int maxIteration = 10;

                // security servers use utc time 
                var recordsTo = DateTime.UtcNow.ToSeconds() - offsetSeconds;

                for (var i = 0; i < maxIteration; i++)
                {
                    _logger.LogInformation(
                        $"Requesting logs from: {server.Identifier} between {server.NextRecordsFromTimestamp} to {recordsTo}");

                    if (TryGetOpData(out var operationalData, server.Identifier,
                        server.NextRecordsFromTimestamp,
                        recordsTo))
                    {
                        _logger.LogInformation(
                            $"Fetched logs {operationalData.Records.Length} from: {server.Identifier} between {server.NextRecordsFromTimestamp} to {recordsTo}");

                        var dataRecords = Array.ConvertAll(operationalData.Records, OpDataRecordExtensions.Convert);

                        ProcessOpDataRecords(server, dataRecords, operationalData.NextRecordsFrom, recordsTo);

                        if (!operationalData.NextRecordsFromSpecified) break;

                        _logger.LogInformation(
                            $"{server.Identifier} - next records from equals: {operationalData.NextRecordsFrom}");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occurred during execution of RunCollector");
            }
        }

        private IEnumerable<Server> GetServersList()
        {
            var serviceScope = _serviceProvider.CreateScope();

            using (serviceScope)
            {
                var serverManager = serviceScope.ServiceProvider.GetRequiredService<IServerManager>();
                return serverManager.GetServersList();
            }
        }

        /// <summary>
        ///     Retrieves operational data from given security server
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
                _logger.LogError($"{securityServerIdentifier}: " + exception.String);
                operationalData = null;
                return false;
            }
        }

        private void ProcessOpDataRecords(Server server, OpDataRecord[] dataRecords, long? nextRecordsFrom,
            long recordsTo)
        {
            var serviceScope = _serviceProvider.CreateScope();
            
            using (serviceScope)
            {
                var serverManager = serviceScope.ServiceProvider.GetService<IServerManager>();
                var opDataStorage = serviceScope.ServiceProvider.GetService<IOpDataStorage>();

                if (!nextRecordsFrom.HasValue)
                    server.NextRecordsFromTimestamp = recordsTo + 1;
                else
                    // ReSharper disable once PossibleInvalidOperationException
                    server.NextRecordsFromTimestamp = nextRecordsFrom.Value;

                opDataStorage.Store(dataRecords);
                serverManager.UpdateServer(server);
            }
        }
    }
}