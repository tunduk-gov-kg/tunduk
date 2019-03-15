using Catalog.DataAccessLayer.Catalog.Entity;
using Catalog.DataAccessLayer.XRoad.Entity;
using Catalog.DataAccessLayer.XRoad.Entity.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<SecurityServer> SecurityServers { get; set; }
        public DbSet<SubSystem> SubSystems { get; set; }
        public DbSet<MemberService> MemberServices { get; set; }
        public DbSet<SubSystemService> SubSystemServices { get; set; }
        public DbSet<MemberInfo> MemberInfoRecords { get; set; }
        public DbSet<MemberRole> MemberRoles { get; set; }
        public DbSet<MemberStatus> MemberStatuses { get; set; }
        public DbSet<MemberType> MemberTypes { get; set; }
        public DbSet<MemberInfoRoleReference> MemberInfoRoleReferences { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
            : base(dbContextOptions) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new MemberServiceConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityServerConfiguration());
            modelBuilder.ApplyConfiguration(new SubSystemConfiguration());
            modelBuilder.ApplyConfiguration(new SubSystemServiceConfiguration());
        }

    }
}
