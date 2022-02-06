using System;

namespace Demo.Adventures.Domain
{
    public class Option
    {
        public Guid Id { get; private set; }
        public string Text { get; set; }
        public Guid NextStepId { get; set; }

        public static Option Create(string text, Guid? nextStepId)
        {
            return new Option
            {
                Id = Guid.NewGuid(),
                Text = text,
                NextStepId = nextStepId ?? Guid.Empty
            };
        }
    }
}