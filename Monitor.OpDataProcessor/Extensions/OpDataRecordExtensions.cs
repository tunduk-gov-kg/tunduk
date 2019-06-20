using System;
using Monitor.Domain.Entity;
using Monitor.Domain.Extensions;

namespace Monitor.OpDataProcessor.Extensions
{
    public static class OpDataRecordExtensions
    {
        public static bool IsConsumer(this OpDataRecord record)
        {
            return record.SecurityServerType.Equals("Client");
        }

        public static bool IsProducer(this OpDataRecord record)
        {
            return record.SecurityServerType.Equals("Producer");
        }

        public static Message CreateMessage(this OpDataRecord record)
        {
            var message = new Message
            {
                CreatedAt = DateTime.Now,
                MessageId = record.MessageId,
                XRequestId = record.XRequestId,
                MessageProtocolVersion = record.MessageProtocolVersion,
                MessageIssue = record.MessageIssue,
                MessageUserId = record.MessageUserId,
                MessageState = record.IsConsumer() ? MessageState.MergedConsumer : MessageState.MergedProducer,

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
                ConsumerSecurityServerInternalIpAddress = record.IsConsumer() ? record.SecurityServerInternalIp : null,
                ConsumerSecurityServerAddress = record.ClientSecurityServerAddress,
                ProducerSecurityServerInternalIpAddress = record.IsProducer() ? record.SecurityServerInternalIp : null,
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

            if (record.IsConsumer())
            {
                message.ConsumerServerRequestIn = record.RequestInTs?.ToDateTime(TemporalType.Milliseconds);
                message.ConsumerServerRequestOut = record.RequestOutTs?.ToDateTime(TemporalType.Milliseconds);
                message.ConsumerServerResponseIn = record.ResponseInTs?.ToDateTime(TemporalType.Milliseconds);
                message.ConsumerServerResponseOut = record.ResponseOutTs?.ToDateTime(TemporalType.Milliseconds);
            }
            else
            {
                message.ProducerServerRequestIn = record.RequestInTs?.ToDateTime(TemporalType.Milliseconds);
                message.ProducerServerRequestOut = record.RequestOutTs?.ToDateTime(TemporalType.Milliseconds);
                message.ProducerServerResponseIn = record.ResponseInTs?.ToDateTime(TemporalType.Milliseconds);
                message.ProducerServerResponseOut = record.ResponseOutTs?.ToDateTime(TemporalType.Milliseconds);
            }

            return message;
        }
    }
}