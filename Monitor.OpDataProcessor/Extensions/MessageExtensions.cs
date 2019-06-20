using System;
using Monitor.Domain.Entity;
using Monitor.Domain.Extensions;

namespace Monitor.OpDataProcessor.Extensions
{
    public static class MessageExtensions
    {
        public static void Merge(this Message message, OpDataRecord record)
        {
            var isConsumer = record.SecurityServerType.Equals("Client");
            
            message.ModifiedAt = DateTime.Now;
            message.MessageState = MessageState.MergedAll;
            
            if (isConsumer)
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
        }
    }
}