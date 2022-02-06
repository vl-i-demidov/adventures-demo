using System;
using Demo.Adventures.Api.Contracts.Adventures;

namespace Demo.Adventures.Api.Contracts.Games
{
    public class SelectOptionRequest
    {
        public Guid StepId { get; set; }
        public Guid OptionId { get; set; }
    }

    public class SelectOptionResponse
    {
        public StepDto NextStep { get; set; }
    }
}