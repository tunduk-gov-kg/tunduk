using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Catalog.BusinessLogicLayer.Service.Interfaces;
using Catalog.DataAccessLayer;
using Catalog.DataAccessLayer.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using XRoad.Domain;

namespace Catalog.BusinessLogicLayer.Service {
    public class MembersStorageUpdater : IXRoadStorageUpdater<MemberData> {
        private readonly AppDbContext _dbContext;

        public MembersStorageUpdater(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Dispose() {
            _dbContext.Dispose();
        }

        public async Task UpdateLocalDatabaseAsync(IImmutableList<MemberData> newMembersList) {
            var databaseMembersList = _dbContext.Members.ToImmutableList();

            RestorePreviouslyRemovedMembers(newMembersList, databaseMembersList);
            RemoveNonExistingMembers(newMembersList, databaseMembersList);
            CreateCompletelyNewMembers(newMembersList, databaseMembersList);

            await _dbContext.SaveChangesAsync();
        }

        private void CreateCompletelyNewMembers(IImmutableList<MemberData> newMembersList,
            ImmutableList<Member> databaseMembersList) {
            foreach (var incomingListMember in newMembersList) {
                var storedInDatabase = databaseMembersList.Any(databaseMember =>
                    Equals(databaseMember, incomingListMember.MemberIdentifier));

                if (!storedInDatabase) {
                    var completelyNewMember = new Member {
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
            ImmutableList<Member> databaseMembersList) {
            foreach (var databaseMember in databaseMembersList) {
                var storedInNewList = newMembersList.Any(incomingListMember =>
                    Equals(databaseMember, incomingListMember.MemberIdentifier));
                if (storedInNewList) continue;
                databaseMember.IsDeleted = true;
                databaseMember.ModificationDateTime = DateTime.Now;
                _dbContext.Members.Update(databaseMember);
            }
        }

        private void RestorePreviouslyRemovedMembers(IImmutableList<MemberData> newMembersList,
            ImmutableList<Member> databaseMembersList) {
            foreach (var databaseMember in databaseMembersList) {
                var shouldBeRestored = databaseMember.IsDeleted &&
                    newMembersList.Any(incomingListMember =>
                        Equals(databaseMember, incomingListMember.MemberIdentifier));
                if (!shouldBeRestored) continue;
                databaseMember.IsDeleted = false;
                databaseMember.ModificationDateTime = DateTime.Now;
                _dbContext.Members.Update(databaseMember);
            }
        }

        private bool Equals(Member member, MemberIdentifier another) {
            return member.Instance.Equals(another.Instance)
                && member.MemberClass.Equals(another.MemberClass)
                && member.MemberCode.Equals(another.MemberCode);
        }


        public async Task UpdateWsdlAsync(ServiceIdentifier serviceIdentifier, string wsdl) {
            var targetService = _dbContext.MemberServices
                .Include(system => system.Member)
                .First(service =>
                    service.ServiceCode == serviceIdentifier.ServiceCode
                    && service.ServiceVersion == serviceIdentifier.ServiceVersion
                    && service.Member.MemberCode == serviceIdentifier.MemberCode
                    && service.Member.MemberClass == serviceIdentifier.MemberClass
                    && service.Member.Instance == serviceIdentifier.Instance);

            targetService.Wsdl = wsdl;
            targetService.ModificationDateTime = DateTime.Now;
            _dbContext.MemberServices.Update(targetService);
            await _dbContext.SaveChangesAsync();
        }
    }
}