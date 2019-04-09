using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.DataAccessLayer;
using Catalog.Domain.Entity;
using Catalog.Domain.Enum;

namespace Catalog.BusinessLogicLayer.Service
{
    public class DomainLogger : IDomainLogger
    {
        private readonly CatalogDbContext _dbContext;

        public DomainLogger(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Log(LogLevel logLevel, string message, string description)
        {
            _dbContext.DomainLogs.Add(new DomainLog
            {
                LogLevel = logLevel,
                Message = message,
                Description = description
            });
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}