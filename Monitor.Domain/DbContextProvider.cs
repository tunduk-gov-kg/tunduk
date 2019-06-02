using Microsoft.EntityFrameworkCore;

namespace Monitor.Domain
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly DbContextOptions<MonitorDbContext> _dbContextOptions;

        public DbContextProvider(string connectionString)
        {
            _dbContextOptions = new DbContextOptionsBuilder<MonitorDbContext>()
                .UseNpgsql(connectionString)
                .Options;
        }

        public MonitorDbContext CreateDbContext()
        {
            return new MonitorDbContext(_dbContextOptions);
        }
    }
}