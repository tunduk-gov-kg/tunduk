using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.DataAccessLayer;
using Catalog.DataAccessLayer.Helpers;
using Catalog.Domain.Entity;
using Catalog.Domain.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.Report
{
    public class ExchangeDataCalculator : IExchangeDataCalculator
    {
        private readonly CatalogDbContext _dbContext;

        public ExchangeDataCalculator(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MemberExchangeData GetExchangeData(Member member, DateTime from, DateTime to)
        {
            var memberExchangeInformation = new MemberExchangeData
            {
                ExchangeData = GetMemberExchangeData(member, from, to),
                SubSystems = member.SubSystems.Select(subSystem => GetExchangeData(subSystem, from, to))
                    .ToList()
            };
            InitializeNames(ref memberExchangeInformation);
            return memberExchangeInformation;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private void InitializeNames(ref MemberExchangeData memberExchangeData)
        {
            void InitializeConsumedServicesNames(List<ConsumedService> consumedServices)
            {
                foreach (var consumedService in consumedServices)
                    consumedService.Name = _dbContext.Services.FindByServiceIdentifier(consumedService.Producer)
                                               ?.NormalizedName ?? consumedService.Producer.ServiceCode;
            }

            void InitializeProducedServicesNames(List<ProducedService> producedServices)
            {
                foreach (var producedService in producedServices)
                {
                    producedService.Name = _dbContext.Services
                                               .FindByServiceIdentifier(producedService.ServiceIdentifier)
                                               ?.NormalizedName ?? producedService.ServiceIdentifier.ServiceCode;

                    foreach (var consumer in producedService.Consumers)
                    {
                        if (consumer.ConsumerIdentifier.SubSystemCode != null)
                        {
                            consumer.Name = _dbContext.SubSystems.FindBySubSystemIdentifier(consumer.ConsumerIdentifier)
                                ?.NormalizedName;
                        }
                        else
                        {
                            consumer.Name = _dbContext.Members.FindByMemberIdentifier(consumer.ConsumerIdentifier).Name;
                        }
                    }
                }
            }

            InitializeConsumedServicesNames(memberExchangeData.ExchangeData.ConsumedServices);
            InitializeProducedServicesNames(memberExchangeData.ExchangeData.ProducedServices);

            foreach (var subSystem in memberExchangeData.SubSystems)
            {
                subSystem.Name = _dbContext.SubSystems.FindBySubSystemIdentifier(subSystem.Identifier).NormalizedName;
                InitializeConsumedServicesNames(subSystem.ConsumedServices);
                InitializeProducedServicesNames(subSystem.ProducedServices);
            }
        }

        private ExchangeData GetMemberExchangeData(Member member, DateTime from, DateTime to)
        {
            var exchangeInformation = new ExchangeData();

            exchangeInformation.Name = member.Name;

            exchangeInformation.Identifier = new SubSystemIdentifier
            {
                Instance = member.Instance,
                MemberClass = member.MemberClass,
                MemberCode = member.MemberCode
            };

            exchangeInformation.ConsumedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereConsumerEquals(member)
                .GroupByProducer()
                .Select(producer => new ConsumedService
                {
                    Producer = producer.Key,
                    RequestsCount = new RequestsCount(
                        producer.Count(message => message.IsSucceeded),
                        producer.Count(message => !message.IsSucceeded))
                }).ToList();

            exchangeInformation.ProducedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereProducerEquals(member)
                .GroupByProducer()
                .Select(producer => new ProducedService
                {
                    ServiceIdentifier = producer.Key,
                    Consumers = producer
                        .GroupBy(message => message.ConsumerIdentifier)
                        .Select(consumer => new Consumer
                        {
                            ConsumerIdentifier = consumer.Key,
                            RequestsCount = new RequestsCount(
                                consumer.Count(it => !it.IsSucceeded),
                                consumer.Count(it => it.IsSucceeded)
                            )
                        }).ToList()
                }).ToList();

            return exchangeInformation;
        }

        public ExchangeData GetExchangeData(SubSystem subSystem, DateTime from, DateTime to)
        {
            var exchangeInformation = new ExchangeData();

            exchangeInformation.Identifier = new SubSystemIdentifier
            {
                Instance = subSystem.Member.Instance,
                MemberClass = subSystem.Member.MemberClass,
                MemberCode = subSystem.Member.MemberCode,
                SubSystemCode = subSystem.SubSystemCode
            };

            exchangeInformation.ConsumedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereConsumerEquals(subSystem)
                .GroupByProducer()
                .Select(producer => new ConsumedService
                {
                    Producer = producer.Key,
                    RequestsCount = new RequestsCount(
                        producer.Count(message => message.IsSucceeded),
                        producer.Count(message => !message.IsSucceeded))
                }).ToList();

            exchangeInformation.ProducedServices = _dbContext.Messages
                .AsExpandable()
                .SelectPeriod(from, to)
                .WhereProducerEquals(subSystem)
                .GroupByProducer()
                .Select(producer => new ProducedService
                {
                    ServiceIdentifier = producer.Key,
                    Consumers = producer
                        .GroupBy(message => message.ConsumerIdentifier)
                        .Select(consumer => new Consumer
                        {
                            ConsumerIdentifier = consumer.Key,
                            RequestsCount = new RequestsCount(
                                consumer.Count(it => !it.IsSucceeded),
                                consumer.Count(it => it.IsSucceeded)
                            )
                        }).ToList()
                }).ToList();

            return exchangeInformation;
        }
    }
}