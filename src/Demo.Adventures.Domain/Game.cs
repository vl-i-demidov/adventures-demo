using System;
using System.Collections.Generic;

namespace Demo.Adventures.Domain
{
    public class Game
    {
        public Guid Id { get; private set; }
        public Guid AdventureId { get; private set; }
        public Guid UserId { get; private set; }
        public List<SelectedOption> SelectedOptions { get; private set; }

        public static Game Create(Guid adventureId, Guid userId)
        {
            return new Game
            {
                Id = Guid.NewGuid(),
                AdventureId = adventureId,
                UserId = userId,
                SelectedOptions = new List<SelectedOption>()
            };
        }
    }
}