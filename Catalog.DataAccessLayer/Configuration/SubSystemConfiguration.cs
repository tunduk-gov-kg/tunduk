using Catalog.Domain.Entity;
using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration {
    internal class SubSystemConfiguration : IEntityTypeConfiguration<SubSystem> {
        public void Configure(EntityTypeBuilder<SubSystem> builder) {
            builder.ConfigureUserTrackableEntity();
            builder.ConfigureSoftDeleteProperties();

            builder.HasIndex(entity => new {
                entity.MemberId,
                entity.SubSystemCode
            }).IsUnique();

            builder.Property(entity => entity.SubSystemCode).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.Name).HasMaxLength(200);
            builder.Property(entity => entity.Description).HasColumnType("text");

            builder.HasMany(entity => entity.Services)
                .WithOne(service => service.SubSystem)
                .HasForeignKey(service => service.SubSystemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(entity => entity.Member)
                .WithMany(subSystem => subSystem.SubSystems)
                .HasForeignKey(subSystem => subSystem.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}