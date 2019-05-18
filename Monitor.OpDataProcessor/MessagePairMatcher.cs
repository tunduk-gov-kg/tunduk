using System.Linq;
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
            //if current record retrieved from producer security server
            //search from consumer messages
            //else search from producer messages
            var searchFor = dataRecord.SecurityServerType.Equals("Producer")
                ? MessageState.MergedConsumer
                : MessageState.MergedProducer;

            if (dataRecord.XRequestId != null)
            {
                return FindMessageByXRequestId(searchFor, dataRecord.XRequestId);
            }

            return DefaultSearch(searchFor, dataRecord);
        }

        private Message DefaultSearch(MessageState targetMessageState, OpDataRecord dataRecord)
        {
            using (var dbContext = _dbContextProvider.CreateDbContext())
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                
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

                var searchInConsumers = targetMessageState == MessageState.MergedConsumer;
                
                if (searchInConsumers)
                {
                    var finalExpression = expression.And(it => it.ConsumerServerRequestOutTs < dataRecord.RequestInTs)
                        .And(it => it.ConsumerServerResponseInTs > dataRecord.ResponseOutTs)
                        .And(it => dataRecord.ResponseOutTs - it.ConsumerServerRequestOutTs >= 60_000);

                    return dbContext.Messages.Where(finalExpression).FirstOrDefault();
                }
                else
                {
                    var finalExpression = expression.And(it => dataRecord.RequestOutTs < it.ProducerServerRequestInTs)
                        .And(it => dataRecord.ResponseInTs > it.ProducerServerResponseOutTs)
                        .And(it => it.ProducerServerResponseOutTs - dataRecord.RequestOutTs >= 60_000);

                    return dbContext.Messages.Where(finalExpression).FirstOrDefault();
                }
            }
        }

        private Message FindMessageByXRequestId(MessageState targetMessageState, string xRequestId)
        {
            using (var dbContext = _dbContextProvider.CreateDbContext())
            {
                dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                
                return dbContext.Messages.FirstOrDefault(it =>
                    it.MessageState.Equals(targetMessageState) && it.XRequestId == xRequestId);
            }
        }
    }
}