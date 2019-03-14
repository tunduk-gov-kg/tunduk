using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.XRoad.Entity.Configuration
{
    internal class MemberServiceConfiguration : IEntityTypeConfiguration<MemberService>
    {
        public void Configure(EntityTypeBuilder<MemberService> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.HasIndex(entity => new
            {
                entity.MemberId,
                entity.ServiceCode,
                entity.ServiceVersion
            }).IsUnique();

            builder.Property(entity => entity.MemberId).IsRequired();
            builder.Property(entity => entity.ServiceCode).IsRequired().HasMaxLength(100);
            builder.Property(entity => entity.ServiceVersion).IsRequired(false).HasMaxLength(100);
            builder.Property(entity => entity.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(entity => entity.Wsdl).IsRequired(false)
                .HasColumnType("text");

            builder.Property(entity => entity.ModificationDateTime).IsRequired(false)
                .ValueGeneratedOnUpdate();

            builder.Property(entity => entity.CreationDateTime).IsRequired()
                .HasDefaultValueSql("now()")
                .ValueGeneratedOnAdd();
            
            builder.HasOne(entity => entity.Member)
                .WithMany(member => member.Services)
                .HasForeignKey(service => service.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
