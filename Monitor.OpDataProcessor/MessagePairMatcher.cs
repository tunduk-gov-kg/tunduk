using System;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Monitor.Domain;
using Monitor.Domain.Entity;

namespace Monitor.OpDataProcessor
{
    public class MessagePairMatcher : IMessagePairMatcher
    {
        private readonly IDbContextProvider _dbContextProvider;

        public MessagePairMatcher(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public Message FindMessage(OpDataRecord dataRecord)
        {
            if (dataRecord.XRequestId != null)
            {
                return FindMessageByXRequestId(dataRecord) ?? DefaultSearch(dataRecord);
            }

            return DefaultSearch(dataRecord);
        }

        private Message DefaultSearch(OpDataRecord dataRecord)
        {
            using (var dbContext = _dbContextProvider.CreateDbContext())
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var searchInProducerRecords = dataRecord.SecurityServerType.Equals("Client");

                var targetMessageState =
                    searchInProducerRecords ? MessageState.MergedProducer : MessageState.MergedConsumer;

                var expression = PredicateBuilder.New<Message>()
                    .And(it => it.MessageState.Equals(targetMessageState))
                    .And(it => it.MessageId == dataRecord.MessageId)
                    .And(it => it.ConsumerInstance == dataRecord.ClientXRoadInstance)
                    .And(it => it.ConsumerMemberClass == dataRecord.ClientMemberClass)
                    .And(it => it.ConsumerSubSystemCode == dataRecord.ClientSubsystemCode)
                    .And(it => it.ProducerInstance == dataRecord.ServiceXRoadInstance)
                    .And(it => it.ProducerMemberClass == dataRecord.ServiceMemberClass)
                    .And(it => it.ProducerMemberCode == dataRecord.ServiceMemberCode)
                    .And(it => it.ProducerServiceCode == dataRecord.ServiceCode);

                const int httpTimeOutMilliSeconds = 60_000;

                Expression<Func<Message, bool>> searchExpression;

                if (searchInProducerRecords)
                {
                    searchExpression = expression.And(it =>
                        it.ProducerServerResponseOutTs - dataRecord.RequestOutTs <= httpTimeOutMilliSeconds);
                }
                else
                {
                    searchExpression = expression.And(it =>
                        dataRecord.ResponseOutTs - it.ConsumerServerRequestOutTs <= httpTimeOutMilliSeconds);
                }

                return dbContext.Messages.Where(searchExpression)
                    .OrderBy(it => it.ConsumerServerRequestOutTs)
                    .FirstOrDefault();
            }
        }

        private Message FindMessageByXRequestId(OpDataRecord dataRecord)
        {
            using (var dbContext = _dbContextProvider.CreateDbContext())
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                return dbContext.Messages.FirstOrDefault(it => it.XRequestId == dataRecord.XRequestId);
            }
        }
    }
}