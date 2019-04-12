using System;
using Catalog.Domain.Entity;
using Catalog.Domain.Enum;

namespace Catalog.Domain.Helpers
{
    public static class MessageHelpers
    {
        public static void MergeOperationalDataRecord(this Message message, OperationalDataRecord operationalDataRecord)
        {
            MessageLifecycle InitMessageLifecycle()
            {
                return new MessageLifecycle
                {
                    RequestInTs = operationalDataRecord.RequestInTs?.AsMilliSecondsToDateTime(),
                    RequestOutTs = operationalDataRecord.RequestOutTs?.AsMilliSecondsToDateTime(),
                    ResponseInTs = operationalDataRecord.ResponseInTs?.AsMilliSecondsToDateTime(),
                    ResponseOutTs = operationalDataRecord.ResponseOutTs?.AsMilliSecondsToDateTime()
                };
            }

            switch (message.MessageState.Name)
            {
                case nameof(MessageState.MergedAll):
                    throw new Exception("Message is already merged!");
                case nameof(MessageState.MergedConsumer):
                    if (operationalDataRecord.IsConsumer)
                        throw new Exception("Cannot merge consumer record twice");
                    message.ProducerSecurityServerInternalIpAddress = operationalDataRecord.SecurityServerInternalIp;
                    message.ProducerMessageLifecycle = InitMessageLifecycle();
                    message.MessageState = MessageState.MergedAll;
                    break;
                case nameof(MessageState.MergedProducer):
                    if (operationalDataRecord.IsProducer)
                        throw new Exception("Cannot merge producer record twice");
                    message.ConsumerSecurityServerInternalIpAddress = operationalDataRecord.SecurityServerInternalIp;
                    message.ConsumerMessageLifecycle = InitMessageLifecycle();
                    message.MessageState = MessageState.MergedAll;
                    break;
                default:
                    throw new Exception("Cannot resolve message state");
            }
        }
    }
}