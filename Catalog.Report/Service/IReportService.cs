using System;
using Catalog.Report.Models;

namespace Catalog.Report.Service
{
    public interface IReportService
    {
        ReportViewModel GenerateReport(DateTime from,DateTime to,bool includeMetaServices);
    }
}