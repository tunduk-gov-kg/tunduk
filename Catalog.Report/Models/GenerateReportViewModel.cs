using System;

namespace Catalog.Report.Models
{
    public class GenerateReportViewModel
    {
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public bool IncludeMetaServices { get; set; }
    }
}