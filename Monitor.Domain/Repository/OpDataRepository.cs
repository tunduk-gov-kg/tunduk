using Monitor.Domain.Entity;

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
    }
}