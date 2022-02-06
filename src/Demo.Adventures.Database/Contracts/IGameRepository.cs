using System;
using System.Threading.Tasks;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Database.Contracts
{
    public interface IGameRepository
    {
        Task<Game> GetGameAsync(Guid gameId);
        Task CreateGameAsync(Game game);
        Task AddSelectedOptionAsync(Guid gameId, SelectedOption option);
    }
}