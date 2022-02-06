using System;
using System.Collections.Generic;

namespace Demo.Adventures.Api.Contracts.Adventures
{
    public class ListStepsResponse
    {
        public List<StepDto> Steps { get; set; }
    }

    public class AdventureDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid FirstStepId { get; set; }
    }

    public class StepDto
    {
        public Guid Id { get; set; }
        public Guid AdventureId { get; set; }
        public string Text { get; set; }

        public List<OptionDto> Options { get; set; }
    }

    public class OptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid NextStepId { get; set; }
    }
}