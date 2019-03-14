using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.XRoad.Entity.Configuration
{
    internal class SubSystemConfiguration : IEntityTypeConfiguration<SubSystem>
    {
        public void Configure(EntityTypeBuilder<SubSystem> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => new
            {
                entity.MemberId,
                entity.SubSystemCode
            }).IsUnique();

            builder.Property(entity => entity.MemberId).IsRequired();
            builder.Property(entity => entity.SubSystemCode).IsRequired().HasMaxLength(100);
            builder.Property(entity => entity.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.Property(entity => entity.CreationDateTime).IsRequired()
                .HasDefaultValueSql("now()");

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
