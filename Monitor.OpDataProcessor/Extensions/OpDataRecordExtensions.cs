using System;
using Monitor.Domain.Entity;

namespace Monitor.OpDataProcessor.Extensions
{
    public static class OpDataRecordExtensions
    {
        public static Message CreateMessage(this OpDataRecord record)
        {
            bool isConsumer = record.SecurityServerType.Equals("Client");

            var message = new Message
            {
                CreatedAt = DateTime.Now,
                MessageId = record.MessageId,
                XRequestId = record.XRequestId,
                MessageProtocolVersion = record.MessageProtocolVersion,
                MessageIssue = record.MessageIssue,
                MessageUserId = record.MessageUserId,
                MessageState = isConsumer ? MessageState.MergedConsumer : MessageState.MergedProducer,

                ConsumerInstance = record.ClientXRoadInstance,
                ConsumerMemberClass = record.ClientMemberClass,
                ConsumerMemberCode = record.ClientMemberCode,
                ConsumerSubSystemCode = record.ClientSubsystemCode,

                ProducerInstance = record.ServiceXRoadInstance,
                ProducerMemberClass = record.ServiceMemberClass,
                ProducerMemberCode = record.ServiceMemberCode,
                ProducerSubSystemCode = record.ServiceSubsystemCode,
                ProducerServiceCode = record.ServiceCode,
                ProducerServiceVersion = record.ServiceVersion,
                ConsumerSecurityServerInternalIpAddress = isConsumer ? record.SecurityServerInternalIp : null,
                ConsumerSecurityServerAddress = record.ClientSecurityServerAddress,
                ProducerSecurityServerInternalIpAddress = !isConsumer ? record.SecurityServerInternalIp : null,
                ProducerSecurityServerAddress = record.ServiceSecurityServerAddress,
                RepresentedPartyClass = record.RepresentedPartyClass,
                RepresentedPartyCode = record.RepresentedPartyCode,

                RequestAttachmentsCount = record.RequestAttachmentCount,
                RequestSoapSize = record.RequestSoapSize,
                RequestMimeSize = record.RequestMimeSize,
                ResponseAttachmentsCount = record.ResponseAttachmentCount,
                ResponseSoapSize = record.ResponseSoapSize,
                ResponseMimeSize = record.ResponseMimeSize,
                IsSucceeded = record.Succeeded ?? false,
                FaultCode = record.SoapFaultCode,
                FaultString = record.SoapFaultString
            };

            if (isConsumer)
            {
                message.ConsumerServerRequestInTs = record.RequestInTs;
                message.ConsumerServerRequestOutTs = record.RequestOutTs;
                message.ConsumerServerResponseInTs = record.ResponseInTs;
                message.ConsumerServerResponseOutTs = record.ResponseOutTs;
            }
            else
            {
                message.ProducerServerRequestInTs = record.RequestInTs;
                message.ProducerServerRequestOutTs = record.RequestOutTs;
                message.ProducerServerResponseInTs = record.ResponseInTs;
                message.ProducerServerResponseOutTs = record.ResponseOutTs;
            }

            return message;
        }
    }
}