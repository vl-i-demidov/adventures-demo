using System;
using System.Threading.Tasks;
using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Tests.Mocks.Database
{
    internal class AdventureRepositoryMock : RepositoryMock<Adventure>, IAdventureRepository
    {
        public Task<Adventure> GetAdventureAsync(Guid adventureId)
        {
            return Task.FromResult(GetEntity(adventureId));
        }

        public Task CreateAdventureAsync(Adventure adventure)
        {
            AddEntity(adventure.Id, adventure);
            return Task.CompletedTask;
        }

        public Task UpdateAdventureAsync(Guid adventureId, string title, Guid? firstStepId)
        {
            var adventure = GetEntity(adventureId);
            if (!string.IsNullOrEmpty(title)) adventure.Title = title;
            if (firstStepId != null) adventure.FirstStepId = firstStepId.Value;

            return Task.CompletedTask;
        }
    }
}