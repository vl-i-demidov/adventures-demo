using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Adventures.Api.Contracts.Adventures
{
    public class CreateStepRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
    }

    public class CreateStepResponse
    {
        public Guid Id { get; set; }
    }
}