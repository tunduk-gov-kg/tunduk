using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Report.Models
{
    public class GenerateBatchReportViewModel
    {
        [Required] public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}