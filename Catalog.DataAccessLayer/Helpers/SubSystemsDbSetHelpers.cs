using System.Linq;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.DataAccessLayer.Helpers
{
    public static class SubSystemsDbSetHelpers
    {
        public static SubSystem FindBySubSystemIdentifier(this IQueryable<SubSystem> source,
            SubSystemIdentifier subSystemIdentifier)
        {
            return source.Include(system => system.Member)
                .SingleOrDefault(system =>
                    system.SubSystemCode == subSystemIdentifier.SubSystemCode
                    && system.Member.Instance == subSystemIdentifier.Instance
                    && system.Member.MemberClass == subSystemIdentifier.MemberClass
                    && system.Member.MemberCode == subSystemIdentifier.MemberCode);
        }
    }
}