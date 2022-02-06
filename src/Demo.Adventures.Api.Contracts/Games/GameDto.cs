using System;
using System.Collections.Generic;
using Demo.Adventures.Api.Contracts.Adventures;

namespace Demo.Adventures.Api.Contracts.Games
{
    public class GameStepDto
    {
        public StepDto Step { get; set; }
        public Guid? SelectedOptionId { get; set; }
    }

    public class GetGameResponse
    {
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public AdventureDto Adventure { get; set; }
        public List<GameStepDto> Steps { get; set; }
    }
}