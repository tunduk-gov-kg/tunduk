using System;
using System.Linq;
using AutoMapper;
using Catalog.DataAccessLayer;
using Catalog.Domain.Entity;
using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleSOAPClient.Exceptions;
using X.PagedList;
using XRoad.Domain;
using XRoad.OpMonitor;
using XRoad.OpMonitor.Domain;
using XRoad.OpMonitor.Domain.SOAP;

namespace Catalog.BusinessLogicLayer.Service
{
    public sealed class XRoadOperationalDataCollector
    {
        private static readonly int MaxIteration = 10;
        private static readonly int OffsetSeconds = 120;
        private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;
        private readonly ILogger<XRoadOperationalDataCollector> _logger;

        private readonly IMapper _mapper;
        private readonly IOperationalDataService _operationalDataService;

        private readonly XRoadExchangeParameters _xRoadExchangeParameters;

        public XRoadOperationalDataCollector(IMapper mapper
            , IOperationalDataService operationalDataService
            , XRoadExchangeParameters xRoadExchangeParameters
            , ILogger<XRoadOperationalDataCollector> logger
            , DbContextOptions<CatalogDbContext> dbContextOptions)
        {
            _operationalDataService = operationalDataService;
            _xRoadExchangeParameters = xRoadExchangeParameters;
            _logger = logger;
            _dbContextOptions = dbContextOptions;
            _mapper = mapper;
        }

        public void RunOpDataCollectorTask()
        {
            var catalogDbContext = new CatalogDbContext(_dbContextOptions);
            var securityServers = catalogDbContext.SecurityServers
                .Include(entity => entity.Member).ToList();
            _logger.LogInformation(LoggingEvents.RunOpdataCollectorTask, "RunOpdataCollector Task started...");

            foreach (var securityServer in securityServers)
            {
                var securityServerIdentifier = _mapper.Map<SecurityServerIdentifier>(securityServer);
                _logger.LogInformation(LoggingEvents.RunOpdataCollectorTask,
                    "Retrieving opdata for security server: {server}",
                    securityServerIdentifier.ToString()
                );

                var nextRecordsFrom = RunOpDataCollectorTask(
                    securityServerIdentifier,
                    securityServer.LastRequestedDateTime ?? 0L.AsSecondsToDateTime()
                );

                securityServer.LastRequestedDateTime = nextRecordsFrom;
                catalogDbContext.SecurityServers.Update(securityServer);
                catalogDbContext.SaveChanges();
            }

            _logger.LogInformation(LoggingEvents.RunOpdataCollectorTask, "RunOpdataCollector Task finished...");
        }


        public DateTime RunOpDataCollectorTask(SecurityServerIdentifier securityServerIdentifier, DateTime from)
        {
            var recordsFrom = from.ToUnixTimestamp();
            var recordsTo = DateTime.Now.ToUnixTimestamp() - OffsetSeconds;

            if (recordsTo - recordsFrom <= 0) throw new ArgumentException("Out of timeline range");

            for (var i = 0; i < MaxIteration; i++)
            {
                var searchCriteria = new SearchCriteria
                {
                    RecordsFrom = recordsFrom,
                    RecordsTo = recordsTo
                };
                _logger.LogInformation(LoggingEvents.GetOperationalData,
                    "Requesting Security Server: {server} for OpData From: {from} To: {to}",
                    securityServerIdentifier,
                    recordsFrom.AsSecondsToDateTime().ToString("s"),
                    recordsTo.AsSecondsToDateTime().ToString("s")
                );

                OperationalData operationalData;

                if (TryGetOperationalData(out operationalData, securityServerIdentifier, searchCriteria))
                {
                    var dataRecords = _mapper.Map<OperationalDataRecord[]>(operationalData.Records);
                    
                    InsertRecordsToDatabase(dataRecords);
                    
                    var shouldBreakTask =
                        operationalData.RecordsCount == 0 || !operationalData.NextRecordsFromSpecified;

                    if (shouldBreakTask)
                    {
                        recordsFrom = recordsTo + 1;
                        break;
                    }
                    // ReSharper disable once PossibleInvalidOperationException
                    recordsFrom = operationalData.NextRecordsFrom.Value;
                }
                else
                {
                    break;
                }
            }

            return recordsFrom.AsSecondsToDateTime();
        }

        private bool TryGetOperationalData(out OperationalData operationalData,
            SecurityServerIdentifier securityServerIdentifier,
            SearchCriteria searchCriteria)
        {
            try
            {
                operationalData = _operationalDataService
                    .GetOperationalData(_xRoadExchangeParameters, securityServerIdentifier, searchCriteria);
                return true;
            }
            catch (FaultException faultException)
            {
                _logger.LogError(LoggingEvents.GetOperationalData,
                    "Error during opdata retrieve operation from security server {id}; " +
                    "Server responded with message: {message}",
                    securityServerIdentifier.ToString(),
                    faultException.String
                );
                operationalData = null;
                return false;
            }
        }

        private void InsertRecordsToDatabase(OperationalDataRecord[] dataRecords)
        {
            try
            {
                var pageNumber = 1;
                var pageSize = 50;
                IPagedList<OperationalDataRecord> pagedList;
                do
                {
                    using (var dbContext = new CatalogDbContext(_dbContextOptions))
                    {
                        pagedList = dataRecords.ToPagedList(pageNumber++, pageSize);
                        dbContext.OperationalDataRecords.AddRange(pagedList);
                        dbContext.SaveChanges();
                    }
                } while (pagedList.HasNextPage);
            }
            catch (DbUpdateException exception)
            {
                _logger.LogError(LoggingEvents.GetOperationalData,
                    "Error during opdata insert operation; Error: " + exception.Message);
            }
        }
    }
}