using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Logic.Contracts
{
    public interface IAdventureService
    {
        Task<Adventure> GetAdventureAsync(Guid adventureId);

        Task<Guid> CreateAdventureAsync(string title);

        Task UpdateAdventureAsync(Guid adventureId, string title, Guid? firstStepId);

        Task<List<Step>> ListStepsAsync(Guid adventureId);

        Task<Step> GetStepAsync(Guid stepId);

        Task<Guid> CreateStepAsync(Guid adventureId, string text);

        Task UpdateStepAsync(Guid stepId, string text);

        Task DeleteStepAsync(Guid stepId);

        Task<Guid> CreateOptionAsync(Guid stepId, string text, Guid? nextStepId);

        Task UpdateOptionAsync(Guid stepId, Guid optionId, string text,
            Guid? nextStepId);

        Task DeleteOptionAsync(Guid stepId, Guid optionId);
    }
}