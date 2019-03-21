using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Domain.Entity.Configuration {
    internal class SubSystemServiceConfiguration : IEntityTypeConfiguration<SubSystemService> {
        public void Configure(EntityTypeBuilder<SubSystemService> builder) {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => new {
                entity.SubSystemId,
                entity.ServiceCode,
                entity.ServiceVersion
            }).IsUnique();

            builder.Property(entity => entity.SubSystemId).IsRequired();
            builder.Property(entity => entity.ServiceCode).IsRequired().HasMaxLength(100);
            builder.Property(entity => entity.ServiceVersion).IsRequired(false).HasMaxLength(100);
            builder.Property(entity => entity.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(entity => entity.Wsdl).IsRequired(false)
                .HasColumnType("text");

            builder.Property(entity => entity.ModifiedAt).IsRequired(false);

            builder.Property(entity => entity.CreatedAt).IsRequired();

            builder.HasOne(entity => entity.SubSystem)
                .WithMany(subSystem => subSystem.Services)
                .HasForeignKey(service => service.SubSystemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}