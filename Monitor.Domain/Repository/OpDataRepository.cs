using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Monitor.Domain.Entity;
using X.PagedList;

namespace Monitor.Domain.Repository
{
    public class OpDataRepository : IOpDataRepository
    {
        private readonly IDbContextProvider _dbContextProvider;

        public OpDataRepository(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public void InsertRecords(OpDataRecord[] opDataRecords)
        {
            using (var dbContext = _dbContextProvider.CreateDbContext())
            {
                dbContext.OpDataRecords.AddRange(opDataRecords);
                dbContext.SaveChanges();
            }
        }

        public IList<OpDataRecord> GetOpDataRecordsBatch(int batchSize, Predicate<OpDataRecord> specification)
        {
            using (var dbContext = _dbContextProvider.CreateDbContext())
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                return dbContext.OpDataRecords.Where(it => specification(it))
                    .ToPagedList(1, batchSize)
                    .ToList();
            }
        }
    }
}