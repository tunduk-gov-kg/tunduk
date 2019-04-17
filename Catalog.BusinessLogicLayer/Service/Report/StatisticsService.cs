using System;
using System.Collections.Generic;
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
            var memberExchangeInformation = new MemberExchangeInformation
            {
                From = from,
                To = to,
                MemberIdentifier = new MemberIdentifier    
                {
                    Instance = member.Instance,
                    MemberClass = member.MemberClass,
                    MemberCode = member.MemberCode
                },
                ExchangeInformation = GetMemberExchangeInformation(member, from, to),
                SubSystems = GetSubSystemsExchangeInformation(member, from, to)
            };
            return memberExchangeInformation;
        }

        private List<SubSystemExchangeInformation> GetSubSystemsExchangeInformation(Member member, DateTime from,
            DateTime to)
        {
            return member.SubSystems.Select(subSystem => new SubSystemExchangeInformation
            {
                ExchangeInformation = GetSubSystemExchangeInformation(subSystem, from, to),
                SubSystemIdentifier = new SubSystemIdentifier
                {
                    Instance = subSystem.Member.Instance,
                    MemberClass = subSystem.Member.MemberClass,
                    MemberCode = subSystem.Member.MemberCode,
                    SubSystemCode = subSystem.SubSystemCode
                }
            }).ToList();
        }


        private ExchangeInformation GetMemberExchangeInformation(Member member, DateTime from, DateTime to)
        {
            var exchangeInformation = new ExchangeInformation();

            exchangeInformation.ConsumedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereConsumerEquals(member, false)
                .GroupByProducer()
                .Select(groupedByService => new ConsumedServiceInformation
                {
                    Consumer = new SubSystemIdentifier
                    {
                        Instance = member.Instance,
                        MemberClass = member.MemberClass,
                        MemberCode = member.MemberCode
                    },
                    Producer = groupedByService.Key,
                    RequestsCount = new RequestsCount(
                        groupedByService.Count(message => message.IsSucceeded),
                        groupedByService.Count(message => !message.IsSucceeded))
                }).ToList();

            exchangeInformation.ProducedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereProducerEquals(member, false)
                .GroupByProducer()
                .Select(groupedByService => new ProducedServiceInformation
                {
                    ServiceIdentifier = groupedByService.Key,
                    Consumers = groupedByService.GroupBy(message => new SubSystemIdentifier
                        {
                            Instance = message.ConsumerInstance,
                            MemberClass = message.ConsumerMemberClass,
                            MemberCode = message.ConsumerMemberCode,
                            SubSystemCode = message.ConsumerSubSystemCode
                        }).Select(groupedByConsumer => new ConsumedServiceInformation
                        {
                            Consumer = groupedByConsumer.Key,
                            Producer = groupedByService.Key,
                            RequestsCount = new RequestsCount(
                                groupedByConsumer.Count(it => !it.IsSucceeded),
                                groupedByConsumer.Count(it => it.IsSucceeded)
                            )
                        })
                        .ToList()
                })
                .ToList();

            return exchangeInformation;
        }

        private ExchangeInformation GetSubSystemExchangeInformation(SubSystem subSystem, DateTime from, DateTime to)
        {
            var exchangeInformation = new ExchangeInformation();

            exchangeInformation.ConsumedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereConsumerEquals(subSystem)
                .GroupByProducer()
                .Select(groupedByService => new ConsumedServiceInformation
                {
                    Consumer = new SubSystemIdentifier
                    {
                        Instance = subSystem.Member.Instance,
                        MemberClass = subSystem.Member.MemberClass,
                        MemberCode = subSystem.Member.MemberCode,
                        SubSystemCode = subSystem.SubSystemCode
                    },
                    Producer = groupedByService.Key,
                    RequestsCount = new RequestsCount(
                        groupedByService.Count(message => message.IsSucceeded),
                        groupedByService.Count(message => !message.IsSucceeded))
                }).ToList();

            exchangeInformation.ProducedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereProducerEquals(subSystem)
                .GroupByProducer()
                .Select(groupedByService => new ProducedServiceInformation
                {
                    ServiceIdentifier = groupedByService.Key,
                    Consumers = groupedByService.GroupBy(message => new SubSystemIdentifier
                        {
                            Instance = message.ConsumerInstance,
                            MemberClass = message.ConsumerMemberClass,
                            MemberCode = message.ConsumerMemberCode,
                            SubSystemCode = message.ConsumerSubSystemCode
                        }).Select(groupedByConsumer => new ConsumedServiceInformation
                        {
                            Consumer = groupedByConsumer.Key,
                            Producer = groupedByService.Key,
                            RequestsCount = new RequestsCount(
                                groupedByConsumer.Count(it => !it.IsSucceeded),
                                groupedByConsumer.Count(it => it.IsSucceeded)
                            )
                        })
                        .ToList()
                })
                .ToList();

            return exchangeInformation;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}