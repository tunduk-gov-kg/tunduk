using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.DataAccessLayer;
using Catalog.Domain.Entity;
using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleSOAPClient.Exceptions;
using XRoad.Domain;
using XRoad.OpMonitor;
using XRoad.OpMonitor.Domain;
using XRoad.OpMonitor.Domain.SOAP;

namespace Catalog.BusinessLogicLayer.Service
{
    public sealed class XRoadOperationalDataCollector
    {
        private static readonly int MaxIteration = 5;
        private static readonly int OffsetSeconds = 180;

        private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;
        private readonly ILogger<XRoadOperationalDataCollector> _logger;
        private readonly IOperationalDataService _operationalDataService;
        private readonly XRoadExchangeParameters _xRoadExchangeParameters;
        private readonly IOperationalDataRepository _repository;
        private readonly IMapper _mapper;

        public XRoadOperationalDataCollector(IMapper mapper
            , IOperationalDataService operationalDataService
            , XRoadExchangeParameters xRoadExchangeParameters
            , ILogger<XRoadOperationalDataCollector> logger
            , DbContextOptions<CatalogDbContext> dbContextOptions
            , IOperationalDataRepository repository)
        {
            _operationalDataService = operationalDataService;
            _xRoadExchangeParameters = xRoadExchangeParameters;
            _logger = logger;
            _dbContextOptions = dbContextOptions;
            _repository = repository;
            _mapper = mapper;
        }

        public void RunOpDataCollectorTask()
        {
            var securityServers = GetSecurityServersList();
            _logger.LogInformation(LoggingEvents.RunOpDataCollectorTask, "RunOpDataCollector Task started.");
            foreach (var securityServer in securityServers) CollectOperationalData(securityServer.Id);
            _logger.LogInformation(LoggingEvents.RunOpDataCollectorTask, "RunOpDataCollector Task finished.");
        }

        private List<SecurityServer> GetSecurityServersList()
        {
            using (var catalogDbContext = new CatalogDbContext(_dbContextOptions))
            {
                return catalogDbContext.SecurityServers
                    .Include(entity => entity.Member)
                    .OrderBy(server => server.Id)
                    .ToList();
            }
        }


        private void CollectOperationalData(long targetSecurityServerId)
        {
            using (var catalogDbContext = new CatalogDbContext(_dbContextOptions))
            {
                using (var transaction = catalogDbContext.Database.BeginTransaction(IsolationLevel.Snapshot))
                {
                    try
                    {
                        var targetSecurityServer = catalogDbContext.SecurityServers
                            .Include(it => it.Member)
                            .First(it => it.Id == targetSecurityServerId);

                        var nextRecordsFrom = RunOpDataCollectorTask(new SecurityServerIdentifier
                            {
                                Instance = targetSecurityServer.Member.Instance,
                                MemberClass = targetSecurityServer.Member.MemberClass,
                                MemberCode = targetSecurityServer.Member.MemberCode,
                                SecurityServerCode = targetSecurityServer.SecurityServerCode,
                            },
                            targetSecurityServer.LastRequestedDateTime ?? 0L.AsSecondsToDateTime()
                        );

                        targetSecurityServer.LastRequestedDateTime = nextRecordsFrom;

                        catalogDbContext.SecurityServers.Update(targetSecurityServer);
                        catalogDbContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "Error occurred during collecting operational data from: {server}",
                            targetSecurityServerId);
                        transaction.Rollback();
                    }
                }
            }
        }

        public DateTime RunOpDataCollectorTask(SecurityServerIdentifier securityServerIdentifier, DateTime from)
        {
            var recordsFrom = from.ToUnixTimestamp();
            var recordsTo = DateTime.UtcNow.ToUnixTimestamp() - OffsetSeconds;

            if (recordsTo - recordsFrom <= 0) throw new ArgumentException("Out of timeline range");

            for (var i = 0; i < MaxIteration; i++)
            {
                _logger.LogInformation(LoggingEvents.GetOperationalData,
                    "Requesting Security Server: {server} for OpData From: {from} To: {to}",
                    securityServerIdentifier, recordsFrom, recordsTo
                );

                if (TryGetOperationalData(out var operationalData, securityServerIdentifier, recordsFrom, recordsTo))
                {
                    _logger.LogInformation(LoggingEvents.GetOperationalData,
                        "Fetched operational data object; Records count: {recordsCount}; Next Records From: {nextRecordsFrom}",
                        operationalData.RecordsCount, operationalData.NextRecordsFrom);

                    var dataRecords = _mapper.Map<OperationalDataRecord[]>(operationalData.Records);
                    if (dataRecords.Length > 0) _repository.InsertRecords(dataRecords);

                    if (!operationalData.NextRecordsFromSpecified)
                    {
                        return (recordsTo + 1).AsSecondsToDateTime();
                    }

                    Debug.Assert(operationalData.NextRecordsFrom != null, "operationalData.NextRecordsFrom != null");
                    recordsFrom = operationalData.NextRecordsFrom.Value;
                }
                else break;
            }

            return recordsFrom.AsSecondsToDateTime();
        }

        private bool TryGetOperationalData(out OperationalData operationalData,
            SecurityServerIdentifier securityServerIdentifier, long recordsFrom, long recordsTo)
        {
            try
            {
                var searchCriteria = new SearchCriteria
                {
                    RecordsFrom = recordsFrom,
                    RecordsTo = recordsTo
                };
                operationalData = _operationalDataService
                    .GetOperationalData(_xRoadExchangeParameters, securityServerIdentifier, searchCriteria);
                return true;
            }
            catch (FaultException faultException)
            {
                _logger.LogError(LoggingEvents.GetOperationalData,
                    "Error occurred during OpData retrieve operation from security server {id}; " +
                    "Error: {message}",
                    securityServerIdentifier.ToString(),
                    faultException.String
                );
                operationalData = null;
                return false;
            }
        }
    }
}