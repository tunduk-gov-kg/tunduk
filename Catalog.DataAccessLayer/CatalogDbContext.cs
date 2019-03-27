using System;
using System.Threading;
using System.Threading.Tasks;
using Catalog.DataAccessLayer.Configuration;
using Catalog.DataAccessLayer.Service;
using Catalog.Domain;
using Catalog.Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.DataAccessLayer {
    public class CatalogDbContext : IdentityDbContext<CatalogUser> {
        private readonly string _currentUserId;
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberRoleReference> MemberRoleReferences { get; set; }
        public DbSet<SecurityServer> SecurityServers { get; set; }
        public DbSet<Domain.Entity.Service> Services { get; set; }
        public DbSet<SubSystem> SubSystems { get; set; }
        public DbSet<DomainLog> DomainLogs { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> dbContextOptions,
            IUserIdProvider<string> userIdProvider)
            : base(dbContextOptions) {
            _currentUserId = userIdProvider.GetCurrentUserId();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CatalogUserConfiguration());
            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new MemberRoleReferenceConfiguration());
            modelBuilder.ApplyConfiguration(new SecurityServerConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new SubSystemConfiguration());
            modelBuilder.ApplyConfiguration(new DomainLogConfiguration());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            ProcessEntitiesBeforeCommit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken()) {
            ProcessEntitiesBeforeCommit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges() {
            ProcessEntitiesBeforeCommit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) {
            ProcessEntitiesBeforeCommit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessEntitiesBeforeCommit() {
            foreach (var entityEntry in ChangeTracker.Entries<ISoftDelete>()) 
                ProcessSoftDeleteEntity(entityEntry);
            foreach (var entityEntry in ChangeTracker.Entries<BaseEntity>()) 
                ProcessBaseEntity(entityEntry);
            foreach (var entityEntry in ChangeTracker.Entries<UserTrackableEntity>()) 
                ProcessUserTrackableEntity(entityEntry);
        }

        private void ProcessUserTrackableEntity(EntityEntry<UserTrackableEntity> entityEntry) {
            if (entityEntry.State == EntityState.Added)
                entityEntry.CurrentValues["CreatedBy"] = _currentUserId;
            else if (entityEntry.State == EntityState.Modified)
                entityEntry.CurrentValues["ModifiedBy"] = _currentUserId;
        }

        private void ProcessSoftDeleteEntity(EntityEntry<ISoftDelete> entity) {
            if (entity.State == EntityState.Deleted) {
                entity.CurrentValues["IsDeleted"] = true;
                entity.State = EntityState.Modified;
            }
            else if (entity.State == EntityState.Added) {
                entity.CurrentValues["IsDeleted"] = false;
            }
        }

        private void ProcessBaseEntity(EntityEntry<BaseEntity> entity) {
            if (entity.State == EntityState.Added) {
                entity.CurrentValues["CreatedAt"] = DateTime.Now;
            }
            else if (entity.State == EntityState.Modified) {
                entity.CurrentValues["ModifiedAt"] = DateTime.Now;
            }
        }
    }
}