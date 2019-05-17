using System;

namespace Monitor.Domain.Entity
{
    public class Message
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public string MessageId { get; set; }
        public string MessageHash { get; set; }
        public string RequestId { get; set; }
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

        public DateTime? ConsumerServerRequestInTs { get; set; }
        public DateTime? ConsumerServerRequestOutTs { get; set; }
        public DateTime? ConsumerServerResponseInTs { get; set; }
        public DateTime? ConsumerServerResponseOutTs { get; set; }

        public DateTime? ProducerServerRequestInTs { get; set; }
        public DateTime? ProducerServerRequestOutTs { get; set; }
        public DateTime? ProducerServerResponseInTs { get; set; }
        public DateTime? ProducerServerResponseOutTs { get; set; }

        public int? RequestAttachmentsCount { get; set; }
        public int? RequestSoapSize { get; set; }
        public int? ResponseAttachmentsCount { get; set; }
        public int? ResponseSoapSize { get; set; }

        public bool IsSucceeded { get; set; }
        public string FaultCode { get; set; }
        public string FaultString { get; set; }
    }
}