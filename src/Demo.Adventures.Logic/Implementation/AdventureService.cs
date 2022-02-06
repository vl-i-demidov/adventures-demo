using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Adventures.Common.Exceptions;
using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Domain;
using Demo.Adventures.Logic.Contracts;

namespace Demo.Adventures.Logic.Implementation
{
    public class AdventureService : IAdventureService
    {
        private readonly IAdventureRepository _adventureRepo;
        private readonly IStepRepository _stepRepo;

        public AdventureService(IAdventureRepository adventureRepo, IStepRepository stepRepo)
        {
            _adventureRepo = adventureRepo;
            _stepRepo = stepRepo;
        }

        public Task<Adventure> GetAdventureAsync(Guid adventureId)
        {
            return _adventureRepo.GetAdventureAsync(adventureId);
        }

        public async Task<Guid> CreateAdventureAsync(string title)
        {
            var adv = Adventure.Create(title);

            await _adventureRepo.CreateAdventureAsync(adv);

            return adv.Id;
        }

        public async Task UpdateAdventureAsync(Guid adventureId, string title, Guid? firstStepId)
        {
            await AssertAdventureExistsAsync(adventureId);

            if (firstStepId != null) await AssertStepExistsAsync(adventureId, firstStepId.Value);

            await _adventureRepo.UpdateAdventureAsync(adventureId, title, firstStepId);
        }

        public async Task<List<Step>> ListStepsAsync(Guid adventureId)
        {
            await AssertAdventureExistsAsync(adventureId);
            return await _stepRepo.ListStepsAsync(adventureId);
        }

        public Task<Step> GetStepAsync(Guid stepId)
        {
            return _stepRepo.GetStepAsync(stepId);
        }

        public async Task<Guid> CreateStepAsync(Guid adventureId, string text)
        {
            await AssertAdventureExistsAsync(adventureId);

            var step = Step.Create(adventureId, text);

            await _stepRepo.CreateStepAsync(step);

            return step.Id;
        }

        public Task UpdateStepAsync(Guid stepId, string text)
        {
            return _stepRepo.UpdateStepAsync(stepId, text);
        }

        public Task DeleteStepAsync(Guid stepId)
        {
            return _stepRepo.DeleteStepAsync(stepId);
        }

        // we only add options, there is currently no way to reorder them
        public async Task<Guid> CreateOptionAsync(Guid stepId, string text, Guid? nextStepId)
        {
            var option = Option.Create(text, nextStepId);

            await _stepRepo.AddOptionAsync(stepId, option);

            return option.Id;
        }

        public Task UpdateOptionAsync(Guid stepId, Guid optionId, string text,
            Guid? nextStepId)
        {
            return _stepRepo.UpdateOptionAsync(stepId, optionId, text, nextStepId);
        }

        public Task DeleteOptionAsync(Guid stepId, Guid optionId)
        {
            return _stepRepo.DeleteOptionAsync(stepId, optionId);
        }

        private Task AssertAdventureExistsAsync(Guid adventureId)
        {
            // throws if adventure doesn't exist
            return GetAdventureAsync(adventureId);
        }

        private async Task AssertStepExistsAsync(Guid adventureId, Guid stepId)
        {
            // throws if step doesn't exist
            var step = await GetStepAsync(stepId);

            // throw if step doesn't belong to adventure
            if (step.AdventureId != adventureId) throw new EntityNotFoundException(stepId, typeof(Step));
        }
    }
}