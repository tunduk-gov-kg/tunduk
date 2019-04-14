using System;
using System.Linq;
using Catalog.BusinessLogicLayer.Service.Report;
using Catalog.DataAccessLayer;
using Catalog.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statistics;
        private readonly CatalogDbContext _dbContext;

        public StatisticsController(IStatisticsService statistics, CatalogDbContext dbContext)
        {
            _statistics = statistics;
            _dbContext = dbContext;
        }

        [HttpGet]
        public MemberExchangeInformation GetMemberExchangeInformation()
        {
            var member = _dbContext.Members.Include(it => it.SubSystems).SingleOrDefault(it => it.Id == 3L);
            return _statistics.GetExchangeInformation(member, DateTime.Now.AddDays(-10), DateTime.Now);
        }
    }
}