using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class DebugController
    {
        [Authorize]
        [HttpGet]
        public async Task<object> Protected()
        {
            return "Protected area";
        }
    }
}