using System;

namespace Demo.Adventures.Domain
{
    public class Adventure
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public Guid FirstStepId { get; set; }

        public static Adventure Create(string title)
        {
            return new Adventure { Id = Guid.NewGuid(), Title = title };
        }
    }
}