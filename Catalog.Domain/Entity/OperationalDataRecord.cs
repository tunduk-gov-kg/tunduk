using System.Runtime.Serialization;

namespace Catalog.Domain.Entity
{
    [DataContract]
    public class OperationalDataRecord : BaseEntity
    {
        public bool IsProcessed { get; set; }

        [DataMember] public string ClientXRoadInstance { get; set; }

        [DataMember] public string ClientMemberClass { get; set; }

        [DataMember] public string ClientMemberCode { get; set; }

        [DataMember] public string ClientSubsystemCode { get; set; }

        [DataMember] public string ClientSecurityServerAddress { get; set; }

        [DataMember] public string ServiceXRoadInstance { get; set; }

        [DataMember] public string ServiceMemberClass { get; set; }

        [DataMember] public string ServiceMemberCode { get; set; }

        [DataMember] public string ServiceSubsystemCode { get; set; }

        [DataMember] public string ServiceCode { get; set; }

        [DataMember] public string ServiceVersion { get; set; }

        [DataMember] public string ServiceSecurityServerAddress { get; set; }


        [DataMember] public string MessageId { get; set; }

        [DataMember] public string MessageIssue { get; set; }

        [DataMember] public string MessageProtocolVersion { get; set; }

        [DataMember] public string MessageUserId { get; set; }

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

        [DataMember] public bool? Succeeded { get; set; }

        [DataMember] public string SoapFaultCode { get; set; }

        [DataMember] public string SoapFaultString { get; set; }

        public bool IsConsumer => SecurityServerType.Equals("Client");

        public bool IsProducer => SecurityServerType.Equals("Producer");
    }
}