using System;
using Catalog.Report.Models;
using Catalog.Report.Service;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Report.Controllers
{
    public class ReportController : Controller
    {
        private IExchangeDataReportService _reportService;

        public ReportController(IExchangeDataReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult GenerateBatchReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateBatchReport(GenerateBatchReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime to = model.To ?? DateTime.Now;
                ViewBag.From = model.From;
                ViewBag.To = to;
                
                var exchangeDataRecords = _reportService.GenerateReport(model.From, to);
                return View("BatchReport", exchangeDataRecords);
            }

            return View();
        }
    }
}