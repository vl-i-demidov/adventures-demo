using System;

namespace Demo.Adventures.Domain
{
    public class GameStep
    {
        public GameStep(Step step, Guid? selectedOptionId = default)
        {
            Step = step;
            SelectedOptionId = selectedOptionId;
        }

        public Step Step { get; }
        public Guid? SelectedOptionId { get; }
    }
}