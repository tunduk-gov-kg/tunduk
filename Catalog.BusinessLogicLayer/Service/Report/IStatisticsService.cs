using System;
using Catalog.Domain.Entity;
using Catalog.Domain.Model;

namespace Catalog.BusinessLogicLayer.Service.Report
{
    public interface IStatisticsService : IDisposable
    {
        MemberExchangeInformation GetExchangeInformation(Member member, DateTime from, DateTime to);
    }
}