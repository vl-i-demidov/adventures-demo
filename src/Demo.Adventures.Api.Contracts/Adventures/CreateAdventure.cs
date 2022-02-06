using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Adventures.Api.Contracts.Adventures
{
    public class CreateAdventureRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
    }

    public class CreateAdventureResponse
    {
        public Guid Id { get; set; }
    }
}