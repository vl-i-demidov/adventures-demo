using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Domain;

namespace Demo.Adventures.Tests.Mocks.Database
{
    internal class StepRepositoryMock : RepositoryMock<Step>, IStepRepository
    {
        public Task<List<Step>> ListStepsAsync(Guid adventureId)
        {
            var steps = _collection.Values.Where(s => s.AdventureId == adventureId).ToList();
            return Task.FromResult(steps);
        }

        public Task<Step> GetStepAsync(Guid stepId)
        {
            return Task.FromResult(GetEntity(stepId));
        }

        public Task CreateStepAsync(Step step)
        {
            AddEntity(step.Id, step);
            return Task.CompletedTask;
        }

        public Task UpdateStepAsync(Guid stepId, string text)
        {
            var step = GetEntity(stepId);
            step.Text = text;

            return Task.CompletedTask;
        }

        public Task DeleteStepAsync(Guid stepId)
        {
            DeleteEntity(stepId);
            return Task.CompletedTask;
        }

        public Task AddOptionAsync(Guid stepId, Option option)
        {
            var step = GetEntity(stepId);
            step.Options.Add(option);

            return Task.CompletedTask;
        }

        public Task UpdateOptionAsync(Guid stepId, Guid optionId, string text, Guid? nextStepId)
        {
            var step = GetEntity(stepId);
            var option = GetOption(step, optionId);

            if (!string.IsNullOrEmpty(text)) option.Text = text;
            if (nextStepId != null) option.NextStepId = nextStepId.Value;

            return Task.CompletedTask;
        }

        public Task DeleteOptionAsync(Guid stepId, Guid optionId)
        {
            // assert exists
            var step = GetEntity(stepId);
            GetOption(step, optionId);

            step.Options.RemoveAll(o => o.Id == optionId);

            return Task.CompletedTask;
        }

        private Option GetOption(Step step, Guid optionId)
        {
            var option = step.Options.FirstOrDefault(o => o.Id == optionId);
            if (option == null) throw GetEntityNotFoundException<Option>(optionId);

            return option;
        }
    }
}