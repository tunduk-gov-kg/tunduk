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
    public class SubSystemsStorageUpdater : IXRoadStorageUpdater<SubSystemIdentifier> {
        private readonly AppDbContext _dbContext;

        public SubSystemsStorageUpdater(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Dispose() {
            _dbContext.Dispose();
        }

        public async Task UpdateLocalDatabaseAsync(IImmutableList<SubSystemIdentifier> subSystemsList) {
            var databaseSubSystemsList = _dbContext.SubSystems
                .Include(subSystem => subSystem.Member)
                .ToImmutableList();

            CreateCompletelyNewSubSystems(subSystemsList, databaseSubSystemsList);
            RemoveNonExistingMembers(subSystemsList, databaseSubSystemsList);
            RestorePreviouslyRemovedMembers(subSystemsList, databaseSubSystemsList);

            await _dbContext.SaveChangesAsync();
        }


        private void CreateCompletelyNewSubSystems(IImmutableList<SubSystemIdentifier> newSubSystemsList,
            ImmutableList<SubSystem> databaseSubSystemsList) {
            foreach (var subSystemIdentifier in newSubSystemsList) {
                var storedInDatabase =
                    databaseSubSystemsList.Any(databaseSubSystem => Equals(databaseSubSystem, subSystemIdentifier));
                if (storedInDatabase) continue;

                var isContextMember = new Predicate<Member>(member =>
                    member.Instance.Equals(subSystemIdentifier.Instance)
                    && member.MemberClass.Equals(subSystemIdentifier.MemberClass)
                    && member.MemberCode.Equals(subSystemIdentifier.MemberCode));

                var contextMember = _dbContext.Members.FirstOrDefault(member => isContextMember(member));

                if (contextMember == null) continue;

                var completelyNewSubSystem = new SubSystem {
                    Member = contextMember,
                    SubSystemCode = subSystemIdentifier.SubSystemCode
                };
                _dbContext.SubSystems.Add(completelyNewSubSystem);
            }
        }

        private void RemoveNonExistingMembers(IImmutableList<SubSystemIdentifier> newSubSystemsList,
            ImmutableList<SubSystem> databaseSubSystemsList) {
            foreach (var databaseSubSystem in databaseSubSystemsList) {
                var storedInSubSystemsList = newSubSystemsList.Any(subSystem => Equals(databaseSubSystem, subSystem));
                if (storedInSubSystemsList) continue;
                databaseSubSystem.IsDeleted = true;
                databaseSubSystem.ModificationDateTime = DateTime.Now;
                _dbContext.SubSystems.Update(databaseSubSystem);
            }
        }

        private void RestorePreviouslyRemovedMembers(IImmutableList<SubSystemIdentifier> newSubSystemsList,
            ImmutableList<SubSystem> databaseSubSystemsList) {
            foreach (var databaseSubSystem in databaseSubSystemsList) {
                if (!databaseSubSystem.IsDeleted) continue;
                var storedInSubSystemsList = newSubSystemsList.Any(subSystem => Equals(databaseSubSystem, subSystem));
                if (!storedInSubSystemsList) continue;
                databaseSubSystem.IsDeleted = false;
                databaseSubSystem.ModificationDateTime = DateTime.Now;
                _dbContext.SubSystems.Update(databaseSubSystem);
            }
        }

        private bool Equals(SubSystem subSystem, SubSystemIdentifier subSystemIdentifier) {
            return subSystem.SubSystemCode.Equals(subSystemIdentifier.SubSystemCode)
                && subSystem.Member.Instance.Equals(subSystemIdentifier.Instance)
                && subSystem.Member.MemberClass.Equals(subSystemIdentifier.MemberClass)
                && subSystem.Member.MemberCode.Equals(subSystemIdentifier.MemberCode);
        }
    }
}