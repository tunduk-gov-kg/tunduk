using System;
using System.Linq;
using AutoMapper;
using Catalog.DataAccessLayer;
using Catalog.Domain.Entity;
using Catalog.Domain.Helpers;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.BusinessLogicLayer.Service
{
    public class XRoadOperationalDataProcessor
    {
        private readonly ILogger<XRoadOperationalDataProcessor> _logger;
        private readonly IMapper _mapper;
        private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;
        private const int ProcessLimit = 10000;

        public void ProcessRecords()
        {
            var catalogDbContext = new CatalogDbContext(_dbContextOptions);
            catalogDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var requireCleanData = PredicateBuilder.New<OperationalDataRecord>()
                .And(it => !it.IsProcessed)
                .And(it => it.SecurityServerType != null)
                .And(it => it.MessageId != null)
                .And(it => it.MessageProtocolVersion != null)
                .And(it => it.ClientXRoadInstance != null)
                .And(it => it.ClientMemberClass != null)
                .And(it => it.ClientMemberCode != null)
                .And(it => it.ServiceXRoadInstance != null)
                .And(it => it.ServiceMemberClass != null)
                .And(it => it.ServiceMemberCode != null)
                .And(it => it.ServiceCode != null);

            var operationalDataRecords = catalogDbContext.OperationalDataRecords.Where(requireCleanData)
                .OrderByDescending(it => it.Id)
                .Take(ProcessLimit)
                .ToList();

            foreach (var operationalDataRecord in operationalDataRecords)
            {
                ProcessOperationalDataRecord(operationalDataRecord);
            }
        }

        public XRoadOperationalDataProcessor(ILogger<XRoadOperationalDataProcessor> logger
            , DbContextOptions<CatalogDbContext> dbContextOptions
            , IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContextOptions = dbContextOptions;
        }

        private void ProcessOperationalDataRecord(OperationalDataRecord record)
        {
            var catalogDbContext = new CatalogDbContext(_dbContextOptions);

            try
            {
                var digest = record.CalculateDigest();
                var foundMessage =
                    catalogDbContext.Messages.FirstOrDefault(message => message.MessageDigest.Equals(digest));

                if (foundMessage == null)
                {
                    var message = _mapper.Map<Message>(record);
                    message.MessageDigest = digest;
                    catalogDbContext.Messages.Add(message);
                }
                else
                {
                    foundMessage.MergeOperationalDataRecord(record);
                    catalogDbContext.Messages.Update(foundMessage);
                }

                catalogDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.OperationalDataRecordProcessing,
                    "Error during processing operational data record with id:{id}; " +
                    "Error message: {message}", record.Id, ex.Message);
            }
            finally
            {
                record.IsProcessed = true;
                catalogDbContext.OperationalDataRecords.Update(record);
                catalogDbContext.SaveChanges();
            }

            catalogDbContext.Dispose();
        }
    }
}