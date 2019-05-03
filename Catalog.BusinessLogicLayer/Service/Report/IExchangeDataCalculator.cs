using System;
using Catalog.Domain.Entity;
using Catalog.Domain.Model;

namespace Catalog.BusinessLogicLayer.Service.Report
{
    public interface IExchangeDataCalculator : IDisposable
    {
        MemberExchangeData GetExchangeData(Member member, DateTime from, DateTime to);
        ExchangeData GetExchangeData(SubSystem subSystem, DateTime from, DateTime to);
    }
}