using System;
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
using XRoad.OpMonitor.Domain.SOAP;

namespace Catalog.BusinessLogicLayer.Service
{
    public class MonitoringService : IMonitoringService
    {
        private static readonly int MaxIteration = 10;
        private static readonly int OffsetSeconds = 120;

        private readonly IMapper _mapper;
        private readonly CatalogDbContext _dbContext;
        private readonly ILogger<MonitoringService> _logger;
        private readonly IOperationalDataService _operationalDataService;

        public MonitoringService(
            CatalogDbContext dbContext
            , IMapper mapper
            , IOperationalDataService operationalDataService
            , XRoadExchangeParameters xRoadExchangeParameters
            , ILogger<MonitoringService> logger)
        {
            _dbContext = dbContext;
            _operationalDataService = operationalDataService;
            _xRoadExchangeParameters = xRoadExchangeParameters;
            _logger = logger;
            _mapper = mapper;
        }

        private readonly XRoadExchangeParameters _xRoadExchangeParameters;

        public void RunOpDataCollectorTask()
        {
            var securityServers = _dbContext.SecurityServers
                .Include(entity => entity.Member).ToList();

            foreach (var securityServer in securityServers)
            {
                var securityServerIdentifier = _mapper.Map<SecurityServerIdentifier>(securityServer);

                var nextRecordsFrom = RunOpDataCollectorTask(
                    securityServerIdentifier,
                    securityServer.LastRequestedDateTime ?? 0L.ToDateTime()
                );

                securityServer.LastRequestedDateTime = nextRecordsFrom;
                _dbContext.SecurityServers.Update(securityServer);
                _dbContext.SaveChanges();
            }
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

                    var operationalData = _operationalDataService
                        .GetOperationalData(_xRoadExchangeParameters, securityServerIdentifier, searchCriteria);

                    var dataRecords = _mapper.Map<OperationalDataRecord[]>(operationalData.Records);
                    _dbContext.OperationalDataRecords.AddRange(dataRecords);
                    _dbContext.SaveChanges();

                    if (!operationalData.NextRecordsFromSpecified)
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
                    "Error during opdata retrieve operation from security server {id}; error message: {message}",
                    securityServerIdentifier.ToString(),
                    faultException.String
                );
            }

            return recordsFrom.ToDateTime();
        }
    }
}