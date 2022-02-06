using System;

namespace Demo.Adventures.Api.Contracts.Games
{
    public class StartGameRequest
    {
        public Guid AdventureId { get; set; }
    }

    public class StartGameResponse
    {
        public Guid GameId { get; set; }
    }
}