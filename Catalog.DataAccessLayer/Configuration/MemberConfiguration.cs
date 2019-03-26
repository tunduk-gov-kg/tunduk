using Catalog.Domain.Entity;
using Catalog.Domain.Enum;
using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration {
    public class MemberConfiguration : IEntityTypeConfiguration<Member> {
        public void Configure(EntityTypeBuilder<Member> builder) {
            builder.ConfigureUserTrackableEntity();
            builder.ConfigureSoftDeleteProperties();

            builder.Property(identifier => identifier.Instance) .IsRequired().HasMaxLength(20);
            builder.Property(identifier => identifier.MemberClass) .IsRequired().HasMaxLength(20);
            builder.Property(identifier => identifier.MemberCode) .IsRequired().HasMaxLength(20);
            
            builder.HasIndex(identifier => new {
                identifier.Instance, 
                identifier.MemberClass, 
                identifier.MemberCode
            }).IsUnique();

            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(200);
            builder.Property(entity => entity.Description).IsRequired(false).HasColumnType("text");
            builder.Property(entity => entity.Site).IsRequired(false).HasMaxLength(500);
            builder.Property(entity => entity.Address).IsRequired(false).HasMaxLength(500);

            builder.Property(entity => entity.MemberStatus)
                .HasMaxLength(200)
                .HasConversion(entity => entity.Name,
                    value => MemberStatus.FromName(value, false));

            builder.Property(entity => entity.MemberType)
                .HasMaxLength(200)
                .HasConversion(entity => entity.Name,
                    value => MemberType.FromName(value, false));

            builder.HasMany(entity => entity.SubSystems)
                .WithOne(subSystem => subSystem.Member)
                .HasForeignKey(subSystem => subSystem.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(entity => entity.SecurityServers)
                .WithOne(server => server.Member)
                .HasForeignKey(server => server.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(entity => entity.MemberRoles)
                .WithOne(reference => reference.Member)
                .HasForeignKey(reference => reference.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}