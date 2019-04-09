using Catalog.Domain.Entity;
using Catalog.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.DataAccessLayer.Configuration
{
    public class OperationalDataRecordConfiguration : IEntityTypeConfiguration<OperationalDataRecord>
    {
        public void Configure(EntityTypeBuilder<OperationalDataRecord> builder)
        {
            builder.ConfigureBaseEntityProperties();
            builder.Property(entity => entity.ClientXRoadInstance).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ClientMemberClass).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ClientMemberCode).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ClientSecurityServerAddress).IsRequired(false).HasMaxLength(200);
            builder.Property(entity => entity.ClientSubsystemCode).IsRequired(false).HasMaxLength(100);

            builder.Property(entity => entity.ServiceXRoadInstance).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ServiceMemberClass).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ServiceMemberCode).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ServiceSubsystemCode).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ServiceCode).IsRequired(false).HasMaxLength(100);
            builder.Property(entity => entity.ServiceVersion).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.ServiceSecurityServerAddress).IsRequired(false).HasMaxLength(200);

            builder.Property(entity => entity.MessageId).IsRequired(false).HasMaxLength(100);
            builder.Property(entity => entity.MessageIssue).IsRequired(false).HasMaxLength(500);
            builder.Property(entity => entity.MessageProtocolVersion).IsRequired(false).HasMaxLength(20);
            builder.Property(entity => entity.MessageUserId).IsRequired(false).HasMaxLength(100);


            builder.Property(entity => entity.MonitoringDataTs).IsRequired(false);
            builder.Property(entity => entity.RepresentedPartyClass).IsRequired(false).HasMaxLength(50);
            builder.Property(entity => entity.RepresentedPartyCode).IsRequired(false).HasMaxLength(50);

            builder.Property(entity => entity.RequestAttachmentCount).IsRequired(false);
            builder.Property(entity => entity.RequestInTs).IsRequired(false);
            builder.Property(entity => entity.RequestOutTs).IsRequired(false);
            builder.Property(entity => entity.RequestSoapSize).IsRequired(false);
            builder.Property(entity => entity.RequestMimeSize).IsRequired(false);

            builder.Property(entity => entity.ResponseAttachmentCount).IsRequired(false);
            builder.Property(entity => entity.ResponseInTs).IsRequired(false);
            builder.Property(entity => entity.ResponseOutTs).IsRequired(false);
            builder.Property(entity => entity.ResponseSoapSize).IsRequired(false);
            builder.Property(entity => entity.ResponseMimeSize).IsRequired(false);

            builder.Property(entity => entity.SecurityServerInternalIp).IsRequired(false);
            builder.Property(entity => entity.SecurityServerType).IsRequired(false);
            builder.Property(entity => entity.Succeeded).IsRequired(false);
            builder.Property(entity => entity.SoapFaultCode).IsRequired(false);
            builder.Property(entity => entity.SoapFaultString).IsRequired(false);
        }
    }
}