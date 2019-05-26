using System;
using System.Diagnostics;
using System.Linq;
using Monitor.Domain.Entity;
using Monitor.Domain.Extensions;
using Monitor.Domain.Repository;
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
        private readonly IServerRepository _repository;
        private readonly IOperationalDataService _opDataReader;
        private readonly IOpDataRepository _opDataRepository;

        public OpDataCollector(IServerRepository repository
            , IOperationalDataService opDataReader
            , XRoadExchangeParameters exchangeParameters
            , IOpDataRepository opDataRepository)
        {
            _repository = repository;
            _opDataReader = opDataReader;
            _exchangeParameters = exchangeParameters;
            _opDataRepository = opDataRepository;
        }

        public void Collect()
        {
            var servers = _repository.GetServersList();
            servers.AsParallel().ForAll(server =>
            {
                try
                {
                    Collect(server);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            });
        }

        private void Collect(Server server)
        {
            const int offsetSeconds = 180;
            const int maxIteration = 5;

            var recordsFrom = server.NextRecordsFrom.ToUnixTimestamp();
            var recordsTo = DateTime.UtcNow.ToUnixTimestamp() - offsetSeconds;

            for (int i = 0; i < maxIteration; i++)
            {
                if (TryGetOpData(out var operationalData, server.GetIdentifier(), recordsFrom, recordsTo))
                {
                    var dataRecords = Array.ConvertAll(operationalData.Records, OpDataRecordExtensions.Convert);
                    if (dataRecords.Length > 0) _opDataRepository.InsertRecords(dataRecords);

                    if (!operationalData.NextRecordsFromSpecified)
                    {
                        recordsFrom = recordsTo + 1;
                        break;
                    }

                    Debug.Assert(operationalData.NextRecordsFrom != null, "operationalData.NextRecordsFrom != null");
                    recordsFrom = operationalData.NextRecordsFrom.Value;
                }
                else break;
            }

            server.NextRecordsFrom = recordsFrom.AsSecondsToDateTime();
            _repository.UpdateServer(server);
        }


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
                Console.WriteLine($"{securityServerIdentifier}: "+exception.String);
                operationalData = null;
                return false;
            }
        }
    }
}