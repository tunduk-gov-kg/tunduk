using System.Linq;
using Catalog.Domain.Entity;

namespace Catalog.DataAccessLayer.Helpers
{
    public static class MessagesDbSetHelpers
    {
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
                return queryable;
            }

            return queryable.Where(message => message.ConsumerSubSystemCode != null);
        }

        public static IQueryable<Message> WhereConsumerEquals(this IQueryable<Message> source, SubSystem subSystem)
        {
            return source.WhereConsumerEquals(subSystem.Member, true)
                .Where(message => message.ConsumerSubSystemCode == subSystem.SubSystemCode);
        }

        public static IQueryable<Message> WhereIsSucceeded(this IQueryable<Message> source, bool value = true)
        {
            return source.Where(message => message.IsSucceeded == value);
        }
    }
}