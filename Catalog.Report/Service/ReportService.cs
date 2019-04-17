using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.BusinessLogicLayer.Service.Report;
using Catalog.DataAccessLayer;
using Catalog.DataAccessLayer.Helpers;
using Catalog.Domain.Enum;
using Catalog.Report.Models;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.Report.Service
{
    public class ReportService : IReportService
    {
        private readonly IStatisticsService _statisticsService;
        private readonly CatalogDbContext _catalogDbContext;

        public ReportService(IStatisticsService statisticsService
            , CatalogDbContext catalogDbContext)
        {
            _statisticsService = statisticsService;
            _catalogDbContext = catalogDbContext;
        }

        public ReportViewModel GenerateReport(DateTime from, DateTime to, bool includeMetaServices)
        {
            var membersList = _catalogDbContext.Members
                .Include(member => member.SubSystems)
                .ToList();

            var exchangeInformations =
                membersList.Select(member => _statisticsService.GetExchangeInformation(member, @from, to));

            var reportViewModel = new ReportViewModel();
            reportViewModel.From = from;
            reportViewModel.To = to;
            reportViewModel.Members = new List<Member>();

            foreach (var memberExchange in exchangeInformations)
            {
                var member = new Member();

                member.Name = _catalogDbContext.Members
                    .FindByMemberIdentifier(memberExchange.MemberIdentifier)
                    .Name;
                member.MetaServices = new List<ProducedService>();
                member.SubSystems = new List<SubSystem>();

                if (includeMetaServices)
                {
                    foreach (var producedService in memberExchange.ExchangeInformation.ProducedServices)
                    {
                        member.MetaServices.Add(new ProducedService
                        {
                            IsMetaService = true,
                            Name = producedService.ServiceIdentifier.ServiceCode,
                            HandledRequests = producedService.RequestsCount.Total
                        });
                    }
                }

                foreach (var subSystemExchange in memberExchange.SubSystems)
                {
                    var contextSubSystem = _catalogDbContext.SubSystems
                        .FindBySubSystemIdentifier(subSystemExchange.SubSystemIdentifier);
                    var subSystem = new SubSystem
                    {
                        Name = contextSubSystem.NormalizedName,
                        ProducedServices = new List<ProducedService>()
                    };

                    foreach (var producedService in subSystemExchange.ExchangeInformation.ProducedServices)
                    {
                        bool isMetaService = IsMetaService(producedService.ServiceIdentifier);

                        if (!includeMetaServices)
                        {
                            if (isMetaService)
                            {
                                continue;
                            }
                        }

                        var contextService = _catalogDbContext.Services.FindByServiceIdentifier(
                            producedService.ServiceIdentifier
                        );

                        var producedServiceModel = new ProducedService
                        {
                            IsMetaService = isMetaService,
                            Name = contextService?.NormalizedName ?? producedService.ServiceIdentifier.ServiceCode,
                            HandledRequests = producedService.RequestsCount.Total
                        };
                        subSystem.ProducedServices.Add(producedServiceModel);
                    }

                    member.SubSystems.Add(subSystem);
                }

                reportViewModel.Members.Add(member);
            }

            reportViewModel.Members = reportViewModel.Members
                .OrderByDescending(it => it.HandledRequestsCount)
                .ToList();

            return reportViewModel;
        }

        private bool IsMetaService(ServiceIdentifier serviceIdentifier)
        {
            return MetaService.IsMetaService(serviceIdentifier.ServiceCode);
        }
    }
}