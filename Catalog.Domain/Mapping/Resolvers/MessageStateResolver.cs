using System;
using AutoMapper;
using Catalog.Domain.Entity;
using Catalog.Domain.Enum;

namespace Catalog.Domain.Mapping.Resolvers
{
    public class MessageStateResolver : IValueResolver<OperationalDataRecord, Message, MessageState>
    {
        public MessageState Resolve(OperationalDataRecord source, Message destination, MessageState destMember,
            ResolutionContext context)
        {
            if (source.IsConsumer) return MessageState.MergedConsumer;

            if (source.IsProducer) return MessageState.MergedProducer;

            throw new Exception("Cannot resolve message state for security server type: " + source.SecurityServerType);
        }
    }
}