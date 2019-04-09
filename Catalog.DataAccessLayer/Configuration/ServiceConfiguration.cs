using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration
{
    internal class ServiceConfiguration : IEntityTypeConfiguration<Domain.Entity.Service>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.Service> builder)
        {
            builder.ConfigureSoftDeleteProperties();
            builder.ConfigureUserTrackableEntity();

            builder.HasIndex(entity => new
            {
                entity.SubSystemId,
                entity.ServiceCode,
                entity.ServiceVersion
            }).IsUnique();

            builder.Property(entity => entity.ServiceCode).IsRequired().HasMaxLength(200);
            builder.Property(entity => entity.ServiceVersion).IsRequired(false).HasMaxLength(20);
            builder.Property(entity => entity.Wsdl).IsRequired(false).HasColumnType("text");

            builder.HasOne(entity => entity.SubSystem)
                .WithMany(subSystem => subSystem.Services)
                .HasForeignKey(service => service.SubSystemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}