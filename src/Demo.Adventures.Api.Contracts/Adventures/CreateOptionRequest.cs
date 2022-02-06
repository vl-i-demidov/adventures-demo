using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Adventures.Api.Contracts.Adventures
{
    public class CreateOptionRequest
    {
        [Required(AllowEmptyStrings = false)] public string Text { get; set; }

        public Guid? NextStepId { get; set; }
    }

    public class CreateOptionResponse
    {
        public Guid Id { get; set; }
    }
}