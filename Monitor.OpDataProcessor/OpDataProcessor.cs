using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Monitor.Domain;
using Monitor.Domain.Entity;
using Monitor.OpDataProcessor.Extensions;
using X.PagedList;

namespace Monitor.OpDataProcessor
{
    public class OpDataProcessor
    {
        private readonly IDbContextProvider _dbContextProvider;
        private readonly IMessagePairMatcher _messagePairMatcher;

        public OpDataProcessor(IDbContextProvider dbContextProvider, IMessagePairMatcher messagePairMatcher)
        {
            _dbContextProvider = dbContextProvider;
            _messagePairMatcher = messagePairMatcher;
        }

        public void ProcessRecords(int batchSize)
        {
            var dbContext = _dbContextProvider.CreateDbContext();
            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var clientDataRecords = dbContext.OpDataRecords
                .Where(it => it.SecurityServerType.Equals("Client") && !it.IsProcessed)
                .OrderBy(it => it.Id)
                .ToPagedList(1, batchSize);

            var producerDataRecords = dbContext.OpDataRecords
                .Where(it => it.SecurityServerType.Equals("Producer") && !it.IsProcessed)
                .OrderBy(it => it.Id)
                .ToPagedList(1, batchSize);

            dbContext.Dispose();

            clientDataRecords.AsParallel().ForAll(ProcessRecord);
            producerDataRecords.AsParallel().ForAll(ProcessRecord);
        }

        private void ProcessRecord(OpDataRecord opDataRecord)
        {
            var dbContext = _dbContextProvider.CreateDbContext();
            try
            {
                if (!opDataRecord.IsValid()) return;

                var message = _messagePairMatcher.FindMessage(opDataRecord);
                if (message == null)
                {
                    message = opDataRecord.CreateMessage();
                    dbContext.Messages.Add(message);
                }
                else
                {
                    if (message.MessageState == MessageState.MergedAll) return;

                    var consumerAlreadyMerged =
                        message.MessageState == MessageState.MergedConsumer && opDataRecord.IsConsumer();
                    var producerAlreadyMerged =
                        message.MessageState == MessageState.MergedProducer && opDataRecord.IsProducer();

                    if (consumerAlreadyMerged || producerAlreadyMerged) return;

                    message.Merge(opDataRecord);
                    dbContext.Messages.Update(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
            finally
            {
                opDataRecord.IsProcessed = true;
                dbContext.OpDataRecords.Update(opDataRecord);
                dbContext.SaveChanges();
                dbContext.Dispose();
            }
        }
    }
}