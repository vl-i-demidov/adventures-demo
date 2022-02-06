using System;

namespace Demo.Adventures.Api.Contracts.Adventures
{
    public class UpdateOptionRequest
    {
        public string Text { get; set; }
        public Guid? NextStepId { get; set; }
    }
}