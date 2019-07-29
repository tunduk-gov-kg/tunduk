using System;
using Monitor.Domain.Entity;
using Monitor.Domain.Extensions;
using XRoad.OpMonitor.Domain;

namespace Monitor.Daemon.Extensions
{
    public static class OpDataRecordExtensions
    {
        public static OpDataRecord Convert(this OperationalDataRecordDto dataRecordDto)
        {
            return new OpDataRecord
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                IsProcessed = false,

                ClientXRoadInstance = dataRecordDto.ClientXRoadInstance,
                ClientMemberClass = dataRecordDto.ClientMemberClass,
                ClientMemberCode = dataRecordDto.ClientMemberCode,
                ClientSubsystemCode = dataRecordDto.ClientSubsystemCode,
                ClientSecurityServerAddress = dataRecordDto.ClientSecurityServerAddress,

                ServiceXRoadInstance = dataRecordDto.ServiceXRoadInstance,
                ServiceMemberClass = dataRecordDto.ServiceMemberClass,
                ServiceMemberCode = dataRecordDto.ServiceMemberCode,
                ServiceSubsystemCode = dataRecordDto.ServiceSubsystemCode,
                ServiceCode = dataRecordDto.ServiceCode,
                ServiceVersion = dataRecordDto.ServiceVersion,
                ServiceSecurityServerAddress = dataRecordDto.ServiceSecurityServerAddress,

                XRequestId = dataRecordDto.XRequestId,
                MessageId = dataRecordDto.MessageId,
                MessageIssue = dataRecordDto.MessageIssue,
                MessageProtocolVersion = dataRecordDto.MessageProtocolVersion,
                MessageUserId = dataRecordDto.MessageUserId,

                MonitoringDataTs = dataRecordDto.MonitoringDataTs?.ToDateTime(TemporalType.Seconds),
                RepresentedPartyCode = dataRecordDto.RepresentedPartyCode,
                RepresentedPartyClass = dataRecordDto.RepresentedPartyClass,

                RequestAttachmentCount = dataRecordDto.RequestAttachmentCount,
                RequestInTs = dataRecordDto.RequestInTs?.ToDateTime(TemporalType.Milliseconds),
                RequestOutTs = dataRecordDto.RequestOutTs?.ToDateTime(TemporalType.Milliseconds),
                RequestSoapSize = dataRecordDto.RequestSoapSize,
                RequestMimeSize = dataRecordDto.RequestMimeSize,

                ResponseAttachmentCount = dataRecordDto.ResponseAttachmentCount,
                ResponseInTs = dataRecordDto.ResponseInTs?.ToDateTime(TemporalType.Milliseconds),
                ResponseOutTs = dataRecordDto.ResponseOutTs?.ToDateTime(TemporalType.Milliseconds),
                ResponseSoapSize = dataRecordDto.ResponseSoapSize,
                ResponseMimeSize = dataRecordDto.ResponseMimeSize,

                SecurityServerInternalIp = dataRecordDto.SecurityServerInternalIp,
                SecurityServerType = dataRecordDto.SecurityServerType,
                Succeeded = dataRecordDto.Succeeded,
                SoapFaultCode = dataRecordDto.SoapFaultCode,
                SoapFaultString = dataRecordDto.SoapFaultString
            };
        }
    }
}