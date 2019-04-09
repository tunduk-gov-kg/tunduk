using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.DataAccessLayer;
using Catalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service
{
    public class MembersStorageUpdater : IXRoadStorageUpdater<MemberData>
    {
        private readonly CatalogDbContext _dbContext;

        public MembersStorageUpdater(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task UpdateLocalDatabaseAsync(IImmutableList<MemberData> newMembersList)
        {
            var databaseMembersList = _dbContext.Members.IgnoreQueryFilters().ToImmutableList();

            RestorePreviouslyRemovedMembers(newMembersList, databaseMembersList);
            RemoveNonExistingMembers(newMembersList, databaseMembersList);
            CreateCompletelyNewMembers(newMembersList, databaseMembersList);

            await _dbContext.SaveChangesAsync();
        }

        private void CreateCompletelyNewMembers(IImmutableList<MemberData> newMembersList,
            ImmutableList<Member> databaseMembersList)
        {
            foreach (var incomingListMember in newMembersList)
            {
                var storedInDatabase = databaseMembersList.Any(databaseMember =>
                    Equals(databaseMember, incomingListMember.MemberIdentifier));

                if (!storedInDatabase)
                {
                    var completelyNewMember = new Member
                    {
                        Instance = incomingListMember.MemberIdentifier.Instance,
                        MemberClass = incomingListMember.MemberIdentifier.MemberClass,
                        MemberCode = incomingListMember.MemberIdentifier.MemberCode,
                        Name = incomingListMember.Name
                    };
                    _dbContext.Members.Add(completelyNewMember);
                }
            }
        }

        private void RemoveNonExistingMembers(IImmutableList<MemberData> newMembersList,
            ImmutableList<Member> databaseMembersList)
        {
            foreach (var databaseMember in databaseMembersList)
            {
                var storedInNewList = newMembersList.Any(incomingListMember =>
                    Equals(databaseMember, incomingListMember.MemberIdentifier));
                if (storedInNewList) continue;
                _dbContext.Members.Remove(databaseMember);
            }
        }

        private void RestorePreviouslyRemovedMembers(IImmutableList<MemberData> newMembersList,
            ImmutableList<Member> databaseMembersList)
        {
            foreach (var databaseMember in databaseMembersList)
            {
                var shouldBeRestored = databaseMember.IsDeleted &&
                    newMembersList.Any(incomingListMember =>
                        Equals(databaseMember, incomingListMember.MemberIdentifier));
                if (!shouldBeRestored) continue;
                databaseMember.IsDeleted = false;
                _dbContext.Members.Update(databaseMember);
            }
        }

        private bool Equals(Member member, MemberIdentifier another)
        {
            return member.Instance.Equals(another.Instance)
                && member.MemberClass.Equals(another.MemberClass)
                && member.MemberCode.Equals(another.MemberCode);
        }
    }
}