using System;
using Catalog.Report.Models;
using Catalog.Report.Service;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Report.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult GenerateReport()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GenerateReport(GenerateReportViewModel generateReportViewModel)
        {
            ViewBag.IncludeMetaServices = generateReportViewModel.IncludeMetaServices;
            var reportViewModel = _reportService.GenerateReport(
                generateReportViewModel.From
                , generateReportViewModel.To ?? DateTime.Now
                , generateReportViewModel.IncludeMetaServices);
            return View("Report", reportViewModel);
        }
    }
}