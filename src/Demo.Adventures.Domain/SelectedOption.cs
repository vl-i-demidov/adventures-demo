using System;

namespace Demo.Adventures.Domain
{
    public class SelectedOption
    {
        public SelectedOption(Guid stepId, Guid optionId)
        {
            StepId = stepId;
            OptionId = optionId;
        }

        public Guid StepId { get; }
        public Guid OptionId { get; }
    }
}