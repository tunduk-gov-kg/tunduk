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

        public static IQueryable<Message> WhereConsumerEquals(this IQueryable<Message> source, Member member,
            bool containsSubSystemCode)
        {
            var queryable = source.Where(
                message => message.ConsumerInstance == member.Instance
                    && message.ConsumerMemberClass == member.MemberClass
                    && message.ConsumerMemberCode == member.MemberCode
            );
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (containsSubSystemCode)
            {
                return queryable.Where(message => message.ConsumerSubSystemCode != null);
            }

            return queryable.Where(message => message.ConsumerSubSystemCode == null);
        }

        public static IQueryable<Message> WhereProducerEquals(this IQueryable<Message> source, Member member,
            bool containsSubSystemCode)
        {
            var queryable = source.Where(
                message => message.ProducerInstance == member.Instance
                    && message.ProducerMemberClass == member.MemberClass
                    && message.ProducerMemberCode == member.MemberCode
            );
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (containsSubSystemCode)
            {
                return queryable.Where(message => message.ProducerSubSystemCode != null);
            }

            return queryable.Where(message => message.ProducerSubSystemCode == null);
        }

        public static IQueryable<Message> WhereConsumerEquals(this IQueryable<Message> source, SubSystem subSystem)
        {
            return source.WhereConsumerEquals(subSystem.Member, true)
                .Where(message => message.ConsumerSubSystemCode == subSystem.SubSystemCode);
        }

        public static IQueryable<Message> WhereProducerEquals(this IQueryable<Message> source, SubSystem subSystem)
        {
            return source.WhereProducerEquals(subSystem.Member, true)
                .Where(message => message.ProducerSubSystemCode == subSystem.SubSystemCode);
        }

        public static IQueryable<Message> WhereIsSucceeded(this IQueryable<Message> source, bool value = true)
        {
            return source.Where(message => message.IsSucceeded == value);
        }
    }
}