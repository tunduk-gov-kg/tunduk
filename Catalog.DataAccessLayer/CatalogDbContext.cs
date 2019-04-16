using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Catalog.DataAccessLayer.Configuration;
using Catalog.Domain;
using Catalog.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.DataAccessLayer
{
    public class CatalogDbContext : IdentityDbContext<CatalogUser>
    {
        private readonly string _currentUserId;

        public CatalogDbContext(DbContextOptions<CatalogDbContext> dbContextOptions,
            IHttpContextAccessor httpContextAccessor = null)
            : base(dbContextOptions)
        {
            var identityName = httpContextAccessor?.HttpContext.User?.Identity.Name;
            var authenticatedUserId =
                httpContextAccessor?.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _currentUserId = identityName ?? authenticatedUserId;
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<MemberRoleReference> MemberRoleReferences { get; set; }
        public DbSet<SecurityServer> SecurityServers { get; set; }
        public DbSet<Domain.Entity.Service> Services { get; set; }
        public DbSet<SubSystem> SubSystems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<OperationalDataRecord> OperationalDataRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole("Administrator"),
                new IdentityRole("CatalogUser")
            });

            modelBuilder.ApplyConfiguration(new CatalogUserConfiguration());
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new MemberRoleReferenceConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityServerConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new SubSystemConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new OperationalDataRecordConfiguration());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ProcessEntitiesBeforeCommit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ProcessEntitiesBeforeCommit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            ProcessEntitiesBeforeCommit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ProcessEntitiesBeforeCommit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessEntitiesBeforeCommit()
        {
            foreach (var entityEntry in ChangeTracker.Entries<ISoftDelete>())
                ProcessSoftDeleteEntity(entityEntry);
            foreach (var entityEntry in ChangeTracker.Entries<BaseEntity>())
                ProcessBaseEntity(entityEntry);
            foreach (var entityEntry in ChangeTracker.Entries<UserTrackableEntity>())
                ProcessUserTrackableEntity(entityEntry);
        }

        private void ProcessUserTrackableEntity(EntityEntry<UserTrackableEntity> entityEntry)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    entityEntry.CurrentValues["CreatedBy"] = _currentUserId;
                    break;
                case EntityState.Modified:
                    entityEntry.CurrentValues["ModifiedBy"] = _currentUserId;
                    break;
            }
        }

        private void ProcessSoftDeleteEntity(EntityEntry<ISoftDelete> entity)
        {
            switch (entity.State)
            {
                case EntityState.Deleted:
                    entity.CurrentValues["IsDeleted"] = true;
                    entity.State = EntityState.Modified;
                    break;
                case EntityState.Added:
                    entity.CurrentValues["IsDeleted"] = false;
                    break;
            }
        }

        private void ProcessBaseEntity(EntityEntry<BaseEntity> entity)
        {
            switch (entity.State)
            {
                case EntityState.Added:
                    entity.CurrentValues["CreatedAt"] = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entity.CurrentValues["ModifiedAt"] = DateTime.Now;
                    break;
            }
        }
    }
}