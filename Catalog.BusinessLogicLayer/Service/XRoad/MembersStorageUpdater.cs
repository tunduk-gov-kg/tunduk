using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.DataAccessLayer;
using Catalog.DataAccessLayer.XRoad.Entity;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service.XRoad
{
    public class MembersStorageUpdater : IXRoadStorageUpdater<MemberData>
    {
        private readonly AppDbContext _dbContext;

        public MembersStorageUpdater(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task UpdateLocalDatabaseAsync(IImmutableList<MemberData> newMembersList)
        {
            var databaseMembersList = _dbContext.Members.ToImmutableList();

            RestorePreviouslyRemovedMembers(newMembersList, databaseMembersList);
            RemoveNonExistingMembers(newMembersList, databaseMembersList);
            CreateCompletelyNewMembers(newMembersList, databaseMembersList);

            await _dbContext.SaveChangesAsync();
        }

        private void CreateCompletelyNewMembers(IImmutableList<MemberData> newMembersList, ImmutableList<Member> databaseMembersList)
        {
            foreach (var incomingListMember in newMembersList)
            {
                bool storedInDatabase = databaseMembersList.Any(databaseMember => Equals(databaseMember, incomingListMember.MemberIdentifier));

                if (!storedInDatabase)
                {
                    var completelyNewMember = new Member()
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

        private void RemoveNonExistingMembers(IImmutableList<MemberData> newMembersList, ImmutableList<Member> databaseMembersList)
        {
            foreach (var databaseMember in databaseMembersList)
            {
                bool storedInNewList = newMembersList.Any(incomingListMember => Equals(incomingListMember.MemberIdentifier, databaseMember));
                if (storedInNewList) continue;
                databaseMember.IsDeleted = true;
                databaseMember.ModificationDateTime = DateTime.Now;
                _dbContext.Members.Update(databaseMember);
            }
        }

        private void RestorePreviouslyRemovedMembers(IImmutableList<MemberData> newMembersList, ImmutableList<Member> databaseMembersList)
        {
            foreach (var databaseMember in databaseMembersList)
            {
                var shouldBeRestored = databaseMember.IsDeleted &&
                    newMembersList.Any(incomingListMember => Equals(incomingListMember.MemberIdentifier, databaseMember));
                if (!shouldBeRestored) continue;
                databaseMember.IsDeleted = false;
                databaseMember.ModificationDateTime = DateTime.Now;
                _dbContext.Members.Update(databaseMember);
            }
        }

        private bool Equals(Member member, MemberIdentifier another)
        {
            return member.Instance.Equals(another.Instance)
                   && member.MemberClass.Equals(another.MemberClass)
                   && member.MemberCode.Equals(another.MemberCode);
        }

        private bool Equals(MemberIdentifier memberIdentifier, Member member)
        {
            return memberIdentifier.Instance.Equals(member.Instance)
                   && memberIdentifier.MemberClass.Equals(member.MemberClass)
                   && memberIdentifier.MemberCode.Equals(member.MemberCode);
        }
    }
}
