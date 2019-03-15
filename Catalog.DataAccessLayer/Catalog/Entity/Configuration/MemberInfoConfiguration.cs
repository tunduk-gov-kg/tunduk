using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Catalog.Entity.Configuration
{
    public class MemberInfoConfiguration : IEntityTypeConfiguration<MemberInfo>
    {
        public void Configure(EntityTypeBuilder<MemberInfo> builder)
        {
            builder.HasKey(entity => entity.MemberInfoId);

            builder.Property(entity => entity.Site).IsRequired(false).HasMaxLength(100);
            builder.Property(entity => entity.Description).HasColumnType("text").IsRequired(false);
            builder.Property(entity => entity.ModificationDateTime).IsRequired(false);

            builder.HasOne(entity => entity.MemberType)
                .WithMany(memberType => memberType.MemberInfoRecords)
                .HasForeignKey(entity => entity.MemberTypeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(entity => entity.MemberStatus)
                .WithMany(memberStatus => memberStatus.MemberInfoRecords)
                .HasForeignKey(entity => entity.MemberStatusId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(entity => entity.Member)
                .WithOne(member => member.MemberInfo)
                .HasForeignKey<MemberInfo>(memberInfo => memberInfo.MemberInfoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(entity => entity.MemberRoleReferences)
                .WithOne(reference => reference.MemberInfo)
                .HasForeignKey(reference => reference.MemberInfoId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}