using System;

namespace Monitor.Domain.Entity
{
    public class OpDataRecord
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsProcessed { get; set; }

        public string ClientXRoadInstance { get; set; }
        public string ClientMemberClass { get; set; }
        public string ClientMemberCode { get; set; }
        public string ClientSubsystemCode { get; set; }
        public string ClientSecurityServerAddress { get; set; }

        public string ServiceXRoadInstance { get; set; }
        public string ServiceMemberClass { get; set; }
        public string ServiceMemberCode { get; set; }
        public string ServiceSubsystemCode { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceVersion { get; set; }
        public string ServiceSecurityServerAddress { get; set; }

        public string XRequestId { get; set; }
        public string MessageId { get; set; }
        public string MessageIssue { get; set; }
        public string MessageProtocolVersion { get; set; }
        public string MessageUserId { get; set; }

        public long? MonitoringDataTs { get; set; }
        public string RepresentedPartyClass { get; set; }
        public string RepresentedPartyCode { get; set; }

        public int? RequestAttachmentCount { get; set; }
        public long? RequestInTs { get; set; }
        public long? RequestOutTs { get; set; }
        public int? RequestSoapSize { get; set; }
        public int? RequestMimeSize { get; set; }

        public int? ResponseAttachmentCount { get; set; }
        public long? ResponseInTs { get; set; }
        public long? ResponseOutTs { get; set; }
        public int? ResponseSoapSize { get; set; }
        public int? ResponseMimeSize { get; set; }

        public string SecurityServerInternalIp { get; set; }
        public string SecurityServerType { get; set; }

        public bool? Succeeded { get; set; }
        public string SoapFaultCode { get; set; }
        public string SoapFaultString { get; set; }
    }
}