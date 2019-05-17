using System;
using Monitor.Domain.Entity;
using XRoad.OpMonitor.Domain;

namespace Monitor.OpDataCollector.Extensions
{
    public static class OpDataRecordExtensions
    {
        public static OpDataRecord Convert(this OperationalDataRecordDto dataRecordDto)
        {
            return new OpDataRecord
            {
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

                MonitoringDataTs = dataRecordDto.MonitoringDataTs,
                RepresentedPartyCode = dataRecordDto.RepresentedPartyCode,
                RepresentedPartyClass = dataRecordDto.RepresentedPartyClass,

                RequestAttachmentCount = dataRecordDto.RequestAttachmentCount,
                RequestInTs = dataRecordDto.RequestInTs,
                RequestOutTs = dataRecordDto.RequestOutTs,
                RequestSoapSize = dataRecordDto.RequestSoapSize,
                RequestMimeSize = dataRecordDto.RequestMimeSize,

                ResponseAttachmentCount = dataRecordDto.ResponseAttachmentCount,
                ResponseInTs = dataRecordDto.ResponseInTs,
                ResponseOutTs = dataRecordDto.ResponseOutTs,
                ResponseSoapSize = dataRecordDto.ResponseSoapSize,
                ResponseMimeSize = dataRecordDto.ResponseMimeSize,

                SecurityServerInternalIp = dataRecordDto.SecurityServerInternalIp,
                SecurityServerType = dataRecordDto.SecurityServerType,
                Succeeded = dataRecordDto.Succeeded,
                SoapFaultCode = dataRecordDto.SoapFaultCode,
                SoapFaultString = dataRecordDto.SoapFaultString,
            };
        }
    }
}