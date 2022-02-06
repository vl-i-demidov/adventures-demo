using System.ComponentModel.DataAnnotations;

namespace Demo.Adventures.Api.Contracts.Adventures
{
    public class UpdateStepRequest
    {
        [Required(AllowEmptyStrings = false)] public string Text { get; set; }
    }
}