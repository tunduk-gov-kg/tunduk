using System;
using System.Linq;
using Catalog.DataAccessLayer;
using Catalog.DataAccessLayer.Helpers;
using Catalog.Domain.Entity;
using Catalog.Domain.Model;
using LinqKit;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.Report
{
    public class StatisticsService : IStatisticsService
    {
        private readonly CatalogDbContext _dbContext;

        public StatisticsService(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MemberExchangeInformation GetExchangeInformation(Member member, DateTime from, DateTime to)
        {
            var memberExchangeInformation = new MemberExchangeInformation();


            throw new NotImplementedException();
        }


        private ExchangeInformation Calculate(Member member)
        {
            var exchangeInformation = new ExchangeInformation();

            _dbContext.Messages
                .AsExpandable()
                .WhereConsumerEquals(member, false)
                .GroupBy(message =>
                    new ServiceIdentifier
                    {
                        Instance = message.ProducerInstance,
                        MemberClass = message.ProducerMemberClass,
                        MemberCode = message.ProducerMemberCode,
                        SubSystemCode = message.ProducerSubSystemCode,
                        ServiceCode = message.ProducerServiceCode,
                        ServiceVersion = message.ProducerServiceVersion
                    })
                .Select(it => new ConsumedServiceInformation
                {
                    Producer = it.Key,
                    RequestsCount = new RequestsCount(
                        it.Count(x => x.IsSucceeded),
                        it.Count(x => !x.IsSucceeded))
                });
            
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}