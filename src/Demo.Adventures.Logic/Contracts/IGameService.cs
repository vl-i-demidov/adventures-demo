using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Logic.Contracts
{
    public interface IGameService
    {
        Task<Guid> StartGameAsync(Guid adventureId, Guid userId);

        Task<Step> SelectOptionAsync(Guid gameId, Guid stepId, Guid optionId);

        Task<Game> GetGameAsync(Guid gameId);

        Task<(Game game, Adventure adventure, List<GameStep> gameSteps)> GetFullGameAsync(Guid gameId);
    }
}