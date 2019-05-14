using System;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.DataAccessLayer;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace Catalog.BusinessLogicLayer.Service
{
    public class OperationalDataRepository : IOperationalDataRepository
    {
        private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;
        private readonly ILogger _logger;

        public OperationalDataRepository(
            DbContextOptions<CatalogDbContext> dbContextOptions,
            ILogger<OperationalDataRepository> logger)
        {
            _dbContextOptions = dbContextOptions;
            _logger = logger;
        }


        public void InsertRecords(OperationalDataRecord[] operationalDataRecords)
        {
            try
            {
                var pageNumber = 1;
                var pageSize = 100;
                IPagedList<OperationalDataRecord> pagedList;
                do
                {
                    using (var dbContext = new CatalogDbContext(_dbContextOptions))
                    {
                        pagedList = operationalDataRecords.ToPagedList(pageNumber++, pageSize);
                        dbContext.OperationalDataRecords.AddRange(pagedList);
                        dbContext.SaveChanges();
                    }
                } while (pagedList.HasNextPage);
            }
            catch (Exception exception)
            {
                _logger.LogError(LoggingEvents.OperationalDataRecordInsert,
                    "Error occurred during OpData records inserting; Error: {error}", exception.Message);
            }
        }
    }
}