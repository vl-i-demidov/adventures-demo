using System;
using System.Threading.Tasks;
using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Tests.Mocks.Database
{
    internal class GameRepositoryMock : RepositoryMock<Game>, IGameRepository
    {
        public Task<Game> GetGameAsync(Guid gameId)
        {
            return Task.FromResult(GetEntity(gameId));
        }

        public Task CreateGameAsync(Game game)
        {
            AddEntity(game.Id, game);
            return Task.CompletedTask;
        }

        public Task AddSelectedOptionAsync(Guid gameId, SelectedOption option)
        {
            var game = GetEntity(gameId);
            game.SelectedOptions.Add(option);

            return Task.CompletedTask;
        }
    }
}