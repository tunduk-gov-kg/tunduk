using System;
using System.Linq;
using Monitor.Domain;
using Monitor.Domain.Entity;
using Monitor.Domain.Repository;
using Monitor.OpDataProcessor.Extensions;

namespace Monitor.OpDataProcessor
{
    public class OpDataProcessor
    {
        private readonly IDbContextProvider _dbContextProvider;
        private readonly IMessagePairMatcher _messagePairMatcher;
        private readonly IOpDataRepository _opDataRepository;

        private readonly Predicate<OpDataRecord> _requireCleanData = record =>
            !record.IsProcessed
            && record.Succeeded != null
            && record.MessageId != null
            && record.ClientXRoadInstance != null
            && record.ClientMemberClass != null
            && record.ClientMemberCode != null
            && record.ServiceXRoadInstance != null
            && record.ServiceMemberClass != null
            && record.ServiceMemberCode != null
            && record.ServiceCode != null;

        public OpDataProcessor(IDbContextProvider dbContextProvider
            , IMessagePairMatcher messagePairMatcher
            , IOpDataRepository opDataRepository)
        {
            _dbContextProvider = dbContextProvider;
            _messagePairMatcher = messagePairMatcher;
            _opDataRepository = opDataRepository;
        }

        public void ProcessRecords()
        {
            var clientDataRecords = _opDataRepository.GetOpDataRecordsBatch(100_000,
                record => _requireCleanData(record) && record.SecurityServerType == "Client");
            clientDataRecords.AsParallel().ForAll(ProcessRecord);
            var producerDataRecords = _opDataRepository.GetOpDataRecordsBatch(100_000,
                record => _requireCleanData(record) && record.SecurityServerType == "Producer");
            producerDataRecords.AsParallel().ForAll(ProcessRecord);
        }

        private void ProcessRecord(OpDataRecord opDataRecord)
        {
            var dbContext = _dbContextProvider.CreateDbContext();
            try
            {
                var message = _messagePairMatcher.FindMessage(opDataRecord);
                if (message == null)
                {
                    message = opDataRecord.CreateMessage();
                    dbContext.Messages.Add(message);
                }
                else
                {
                    if (message.MessageState == MessageState.MergedAll)
                    {
                        return;
                    }
                    
                    bool consumerAlreadyMerged = message.MessageState == MessageState.MergedConsumer && opDataRecord.IsConsumer();
                    bool producerAlreadyMerged = message.MessageState == MessageState.MergedProducer && opDataRecord.IsProducer();

                    if (consumerAlreadyMerged || producerAlreadyMerged)
                    {
                        return;
                    }

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