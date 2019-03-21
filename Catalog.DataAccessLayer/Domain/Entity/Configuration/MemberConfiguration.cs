using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Domain.Entity.Configuration {
    internal class MemberConfiguration : IEntityTypeConfiguration<Member> {
        public void Configure(EntityTypeBuilder<Member> builder) {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => new {
                entity.Instance,
                entity.MemberClass,
                entity.MemberCode
            }).IsUnique();

            builder.Property(entity => entity.Instance).IsRequired().HasMaxLength(20);
            builder.Property(entity => entity.MemberClass).IsRequired().HasMaxLength(20);
            builder.Property(entity => entity.MemberCode).IsRequired().HasMaxLength(20);
            builder.Property(entity => entity.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(entity => entity.Name).IsRequired().HasMaxLength(1000);

            builder.Property(entity => entity.CreatedAt).IsRequired()
                .HasDefaultValueSql("now()");

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

            builder.HasMany(entity => entity.Services)
                .WithOne(service => service.Member)
                .HasForeignKey(service => service.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(entity => entity.MemberInfo)
                .WithOne(info => info.Member)
                .HasForeignKey<MemberInfo>(entity => entity.MemberInfoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}