using Microsoft.EntityFrameworkCore;
using Monitor.Domain.Entity;

namespace Monitor.Domain
{
    public class MonitorDbContext : DbContext
    {
        public MonitorDbContext(DbContextOptions<MonitorDbContext> options)
            : base(options)
        {
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<OpDataRecord> OpDataRecords { get; set; }
    }
}