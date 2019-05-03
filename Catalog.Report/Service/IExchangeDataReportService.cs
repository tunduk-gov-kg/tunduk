using System;
using System.Collections.Generic;
using Catalog.Domain.Model;

namespace Catalog.Report.Service
{
    public interface IExchangeDataReportService
    {
        List<MemberExchangeData> GenerateReport(DateTime from, DateTime to);
        List<MemberExchangeData> GenerateMetadataReport(DateTime from, DateTime to);
    }
}