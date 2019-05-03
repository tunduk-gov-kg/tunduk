using Microsoft.AspNetCore.Mvc;

namespace Catalog.Report.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}