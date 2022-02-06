using System;
using System.Collections.Generic;

namespace Demo.Adventures.Domain
{
    public class Step
    {
        public Guid Id { get; private set; }
        public Guid AdventureId { get; private set; }
        public string Text { get; set; }

        public List<Option> Options { get; private set; }

        public static Step Create(Guid adventureId, string text)
        {
            return new Step
            {
                Id = Guid.NewGuid(),
                AdventureId = adventureId,
                Text = text,
                Options = new List<Option>()
            };
        }
    }
}