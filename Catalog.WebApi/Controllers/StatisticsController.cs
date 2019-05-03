using System;
using System.Linq;
using System.Web.Http;
using Catalog.BusinessLogicLayer.Service.Report;
using Catalog.DataAccessLayer;
using Catalog.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.WebApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly CatalogDbContext _dbContext;
        private readonly IExchangeDataCalculator _statistics;

        public StatisticsController(IExchangeDataCalculator statistics, CatalogDbContext dbContext)
        {
            _statistics = statistics;
            _dbContext = dbContext;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public MemberExchangeData GetMemberExchangeInformation(
            [FromUri] long memberId,
            [FromUri] DateTime from,
            [FromUri] DateTime? to = null
        )
        {
            var member = _dbContext.Members.Include(it => it.SubSystems)
                .Single(it => it.Id == memberId);
            return _statistics.GetExchangeData(member, from, to ?? DateTime.Now);
        }
    }
}