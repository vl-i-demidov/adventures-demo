using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Database.Contracts
{
    public interface IStepRepository
    {
        Task<List<Step>> ListStepsAsync(Guid adventureId);
        Task<Step> GetStepAsync(Guid stepId);
        Task CreateStepAsync(Step step);
        Task UpdateStepAsync(Guid stepId, string text);
        Task DeleteStepAsync(Guid stepId);

        Task AddOptionAsync(Guid stepId, Option option);
        Task UpdateOptionAsync(Guid stepId, Guid optionId, string text, Guid? nextStepId);
        Task DeleteOptionAsync(Guid stepId, Guid optionId);
    }
}