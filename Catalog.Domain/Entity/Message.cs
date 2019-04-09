using Catalog.Domain.Enum;

namespace Catalog.Domain.Entity
{
    public class Message : BaseEntity
    {
        public string MessageId { get; set; }
        public string MessageDigest { get; set; }
        public string MessageProtocolVersion { get; set; }
        public string MessageIssue { get; set; }
        public string MessageUserId { get; set; }
        public MessageState MessageState { get; set; }

        public string ConsumerInstance { get; set; }
        public string ConsumerMemberClass { get; set; }
        public string ConsumerMemberCode { get; set; }
        public string ConsumerSubSystemCode { get; set; }

        public string ProducerInstance { get; set; }
        public string ProducerMemberClass { get; set; }
        public string ProducerMemberCode { get; set; }
        public string ProducerSubSystemCode { get; set; }
        public string ProducerServiceCode { get; set; }
        public string ProducerServiceVersion { get; set; }

        public string ConsumerSecurityServerInternalIpAddress { get; set; }
        public string ConsumerSecurityServerAddress { get; set; }
        public string ProducerSecurityServerInternalIpAddress { get; set; }
        public string ProducerSecurityServerAddress { get; set; }

        public MessageLifecycle ConsumerMessageLifecycle { get; set; }
        public MessageLifecycle ProducerMessageLifecycle { get; set; }

        public int? RequestAttachmentsCount { get; set; }
        public int? RequestSoapSize { get; set; }
        public int? ResponseAttachmentsCount { get; set; }
        public int? ResponseSoapSize { get; set; }

        public bool IsSucceeded { get; set; }
        public string FaultCode { get; set; }
        public string FaultString { get; set; }
    }
}