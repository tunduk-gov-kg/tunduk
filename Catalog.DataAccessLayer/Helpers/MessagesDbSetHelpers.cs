using System;
using System.Linq;
using Catalog.Domain.Entity;
using XRoad.Domain;

namespace Catalog.DataAccessLayer.Helpers
{
    public static class MessagesDbSetHelpers
    {
        public static IQueryable<Message> SelectPeriod(this IQueryable<Message> source, DateTime from, DateTime to)
        {
            return source.Where(message => message.ConsumerMessageLifecycle.RequestInTs != null)
                .Where(message => message.ConsumerMessageLifecycle.RequestInTs >= from)
                .Where(message => message.ConsumerMessageLifecycle.RequestInTs <= to);
        }

        public static IQueryable<IGrouping<ServiceIdentifier, Message>> GroupByProducer(this IQueryable<Message> source)
        {
            return source.GroupBy(message =>
                new ServiceIdentifier
                {
                    Instance = message.ProducerInstance,
                    MemberClass = message.ProducerMemberClass,
                    MemberCode = message.ProducerMemberCode,
                    SubSystemCode = message.ProducerSubSystemCode,
                    ServiceCode = message.ProducerServiceCode,
                    ServiceVersion = message.ProducerServiceVersion
                });
        }

        public static IQueryable<Message> WhereConsumerEquals(this IQueryable<Message> source, Member member)
        {
            return source.Where(
                message => message.ConsumerSubSystemCode == null
                           && message.ConsumerMemberCode == member.MemberCode
                           && message.ConsumerMemberClass == member.MemberClass
                           && message.ConsumerInstance == member.Instance
            );
        }

        public static IQueryable<Message> WhereProducerEquals(this IQueryable<Message> source, Member member)
        {
            return source.Where(
                message => message.ProducerSubSystemCode == null
                           && message.ProducerMemberCode == member.MemberCode
                           && message.ProducerMemberClass == member.MemberClass
                           && message.ProducerInstance == member.Instance
            );
        }

        public static IQueryable<Message> WhereConsumerEquals(this IQueryable<Message> source, SubSystem subSystem)
        {
            return source.Where(
                message => message.ConsumerSubSystemCode == subSystem.SubSystemCode
                           && message.ConsumerMemberCode == subSystem.Member.MemberCode
                           && message.ConsumerMemberClass == subSystem.Member.MemberClass
                           && message.ConsumerInstance == subSystem.Member.Instance
            );
        }

        public static IQueryable<Message> WhereProducerEquals(this IQueryable<Message> source, SubSystem subSystem)
        {
            return source.Where(
                message => message.ProducerSubSystemCode == subSystem.SubSystemCode
                           && message.ProducerMemberCode == subSystem.Member.MemberCode
                           && message.ProducerMemberClass == subSystem.Member.MemberClass
                           && message.ProducerInstance == subSystem.Member.Instance
            );
        }
    }
}