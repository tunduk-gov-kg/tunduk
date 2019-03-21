using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Domain.Entity.Configuration {
    public class MemberInfoRoleReferenceConfiguration : IEntityTypeConfiguration<MemberInfoRoleReference> {
        public void Configure(EntityTypeBuilder<MemberInfoRoleReference> builder) {
            builder.HasKey(entity => new {
                entity.MemberRoleId,
                entity.MemberInfoId
            });

            builder.HasOne(entity => entity.MemberInfo)
                .WithMany(memberInfo => memberInfo.MemberRoleReferences)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(entity => entity.MemberInfoId);

            builder.HasOne(entity => entity.MemberRole)
                .WithMany(memberRole => memberRole.MemberInfoReferences)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(entity => entity.MemberRoleId);
        }
    }
}