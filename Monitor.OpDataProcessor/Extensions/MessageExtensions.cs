using System;
using Monitor.Domain.Entity;

namespace Monitor.OpDataProcessor.Extensions
{
    public static class MessageExtensions
    {
        public static void Merge(this Message message, OpDataRecord record)
        {
            var isConsumer = record.SecurityServerType.Equals("Client");

            if (isConsumer && message.MessageState == MessageState.MergedConsumer) throw new Exception(nameof(record));

            message.ModifiedAt = DateTime.Now;
            message.MessageState = MessageState.MergedAll;

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
        }
    }
}