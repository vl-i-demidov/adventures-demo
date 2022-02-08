using System;

namespace Demo.Adventures.Domain
{
    public class SelectedOption
    {
        public SelectedOption(Guid optionId)
        {
            OptionId = optionId;
        }

        public Guid OptionId { get; }
    }
}