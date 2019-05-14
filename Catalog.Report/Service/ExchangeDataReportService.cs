using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.BusinessLogicLayer.Service.Report;
using Catalog.DataAccessLayer;
using Catalog.Domain.Enum;
using Catalog.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Report.Service
{
    public class ExchangeDataReportService : IExchangeDataReportService
    {
        private readonly DbContextOptions<CatalogDbContext> _dbContextOptions;

        public ExchangeDataReportService(DbContextOptions<CatalogDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }


        public List<MemberExchangeData> GenerateReport(DateTime from, DateTime to)
        {
            var dbContext = new CatalogDbContext(_dbContextOptions);
            var members = dbContext.Members
                .Include(member => member.SubSystems)
                .ToList();
            dbContext.Dispose();

            var query = members.AsParallel().Select(member =>
                {
                    using (var dataCalculator = new ExchangeDataCalculator(new CatalogDbContext(_dbContextOptions)))
                    {
                        var memberExchangeData = dataCalculator.GetExchangeData(member, from.ToUniversalTime(), to.ToUniversalTime());
                        CleanMetadataExchange(ref memberExchangeData);
                        return memberExchangeData;
                    }
                })
                .ToList();
            
            return query
                .OrderByDescending(it => it.TotalIncomingRequestsCount)
                .ThenByDescending(it=>it.TotalOutgoingRequestsCount)
                .ThenByDescending(it=>it.ExchangeData.Name)
                .ToList();
        }

        public List<MemberExchangeData> GenerateMetadataReport(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }


        private void CleanMetadataExchange(ref MemberExchangeData memberExchangeData)
        {
            memberExchangeData.ExchangeData.ConsumedServices.RemoveAll(IsMetaService);
            memberExchangeData.ExchangeData.ProducedServices.RemoveAll(IsMetaService);
            foreach (var subSystem in memberExchangeData.SubSystems)
            {
                subSystem.ConsumedServices.RemoveAll(IsMetaService);
                subSystem.ProducedServices.RemoveAll(IsMetaService);
            }
        }

        private bool IsMetaService(ProducedService producedService) =>
            MetaService.IsMetaService(producedService.ServiceIdentifier.ServiceCode);

        private bool IsMetaService(ConsumedService consumedService) =>
            MetaService.IsMetaService(consumedService.Producer.ServiceCode);
    }
}