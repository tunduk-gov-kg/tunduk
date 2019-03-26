using Catalog.Domain.Entity;
using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration {
    internal class SecurityServerConfiguration : IEntityTypeConfiguration<SecurityServer> {
        public void Configure(EntityTypeBuilder<SecurityServer> builder) {
            builder.ConfigureSoftDeleteProperties();
            builder.ConfigureSoftDeleteProperties();

            builder.HasIndex(entity => new {
                entity.MemberId,
                entity.SecurityServerCode
            }).IsUnique();

            builder.Property(entity => entity.SecurityServerCode).IsRequired().HasMaxLength(200);
            builder.Property(entity => entity.Address).IsRequired().HasMaxLength(500);

            builder.HasOne(server => server.Member)
                .WithMany(member => member.SecurityServers)
                .IsRequired()
                .HasForeignKey(entity => entity.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}