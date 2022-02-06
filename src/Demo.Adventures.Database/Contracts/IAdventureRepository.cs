using System;
using System.Threading.Tasks;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Database.Contracts
{
    public interface IAdventureRepository
    {
        Task<Adventure> GetAdventureAsync(Guid adventureId);
        Task CreateAdventureAsync(Adventure adventure);
        Task UpdateAdventureAsync(Guid adventureId, string title, Guid? firstStepId);
    }
}
