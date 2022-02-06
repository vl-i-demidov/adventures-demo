using System;

namespace Demo.Adventures.Api.Contracts.Adventures
{
    public class UpdateAdventureRequest
    {
        public string Title { get; set; }
        public Guid? FirstStepId { get; set; }
    }
}