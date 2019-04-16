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
using XRoad.OpMonitor.Domain.SOAP;

namespace Catalog.BusinessLogicLayer.Service
{
    public sealed class XRoadOperationalDataCollector
    {
        private static readonly int MaxIteration = 10;
        private static readonly int OffsetSeconds = 120;

        private readonly IMapper _mapper;
        private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;
        private readonly ILogger<XRoadOperationalDataCollector> _logger;
        private readonly IOperationalDataService _operationalDataService;

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

        private readonly XRoadExchangeParameters _xRoadExchangeParameters;

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
            var recordsTo   = DateTime.Now.ToUnixTimestamp() - OffsetSeconds;

            if (recordsTo - recordsFrom <= 0) throw new ArgumentException("Out of timeline range");

            try
            {
                for (var i = 0; i < MaxIteration; i++)
                {
                    var searchCriteria = new SearchCriteria
                    {
                        RecordsFrom = recordsFrom,
                        RecordsTo = recordsTo
                    };
                    _logger.LogInformation(LoggingEvents.GetOperationalData,
                        "Requesting Security Server: {server} for OpData From: {from} To: {to}",
                        securityServerIdentifier, recordsFrom, recordsTo
                    );

                    var operationalData = _operationalDataService
                        .GetOperationalData(_xRoadExchangeParameters, securityServerIdentifier, searchCriteria);

                    var dataRecords = _mapper.Map<OperationalDataRecord[]>(operationalData.Records);

                    int pageNumber = 1;
                    int pageSize   = 50;

                    IPagedList<OperationalDataRecord> pagedList;

                    do
                    {
                        using (var dbContext = new CatalogDbContext(_dbContextOptions))
                        {
                            pagedList = dataRecords.ToPagedList(pageNumber++, pageSize);
                            dbContext.OperationalDataRecords.AddRange(pagedList);
                            dbContext.SaveChanges();
                        }

                        _logger.LogInformation("{PageNumber}", pageNumber);
                    } while (pagedList.HasNextPage);

                    bool shouldBreakTask =
                        !operationalData.NextRecordsFromSpecified || operationalData.RecordsCount.Equals(0);

                    if (shouldBreakTask)
                    {
                        recordsFrom = recordsTo + 1;
                        break;
                    }

                    // ReSharper disable once PossibleInvalidOperationException
                    recordsFrom = operationalData.NextRecordsFrom.Value;
                }
            }
            catch (FaultException faultException)
            {
                _logger.LogError(LoggingEvents.GetOperationalData,
                    "Error during opdata retrieve operation from security server {id}; " +
                    "Server responded with message: {message}",
                    securityServerIdentifier.ToString(),
                    faultException.String
                );
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(LoggingEvents.GetOperationalData,
                    "Error during opdata insert operation; security server {id}" + securityServerIdentifier
                    + "; Error: " + dbUpdateException.Message);
            }

            return recordsFrom.AsSecondsToDateTime();
        }
    }
}