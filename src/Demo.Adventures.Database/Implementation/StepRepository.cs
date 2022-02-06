using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Demo.Adventures.Database.Implementation
{
    public class StepRepository : Repository<Step>, IStepRepository
    {
        static StepRepository()
        {
            BsonClassMap.RegisterClassMap<StepRepository>();
        }

        public StepRepository(IOptions<RepositoryConfig> config) :
            base(config, "step")
        {
        }

        public async Task<List<Step>> ListStepsAsync(Guid adventureId)
        {
            var filter = Builders<Step>.Filter.Eq(s => s.AdventureId, adventureId);

            var stepCursor = await Collection.FindAsync(filter);
            var steps = await stepCursor.ToListAsync();

            return steps;
        }

        public async Task<Step> GetStepAsync(Guid stepId)
        {
            var filter = CreateStepFilter(stepId);

            var stepCursor = await Collection.FindAsync(filter);
            var step = await stepCursor.FirstOrDefaultAsync();

            AssertExists<Step>(() => step != null, stepId);

            return step;
        }

        public async Task CreateStepAsync(Step step)
        {
            await Collection.InsertOneAsync(step);
        }

        public async Task UpdateStepAsync(Guid stepId, string text)
        {
            var filter = CreateStepFilter(stepId);
            var update = Builders<Step>.Update.Set(s => s.Text, text);

            var result = await Collection.UpdateOneAsync(filter, update);

            AssertExists<Step>(result, stepId);
        }

        public async Task DeleteStepAsync(Guid stepId)
        {
            var result = await Collection.DeleteOneAsync(CreateStepFilter(stepId));

            AssertExists<Step>(result, stepId);
        }

        public async Task AddOptionAsync(Guid stepId, Option option)
        {
            var filter = CreateStepFilter(stepId);
            var update = Builders<Step>.Update.Push(s => s.Options, option);
            var result = await Collection.UpdateOneAsync(filter, update);

            AssertExists<Step>(result, stepId);
        }

        public async Task UpdateOptionAsync(Guid stepId, Guid optionId, string text, Guid? nextStepId)
        {
            var filter = CreateStepFilter(stepId)
                         & Builders<Step>.Filter.ElemMatch(x => x.Options,
                             Builders<Option>.Filter.Eq(x => x.Id, optionId));
            var update = Builders<Step>.Update;

            var definitions = new List<UpdateDefinition<Step>>();
            if (!string.IsNullOrEmpty(text)) definitions.Add(update.Set(s => s.Options[-1].Text, text));
            if (nextStepId != null) definitions.Add(update.Set(s => s.Options[-1].NextStepId, nextStepId));

            var result = await Collection.UpdateOneAsync(filter, update.Combine(definitions));

            AssertExists<Option>(result, stepId);
        }

        public async Task DeleteOptionAsync(Guid stepId, Guid optionId)
        {
            var update = Builders<Step>.Update.PullFilter(s => s.Options, o => o.Id == optionId);
            var result = await Collection.UpdateOneAsync(p => p.Id == stepId, update);

            AssertExists<Option>(result, stepId);
        }

        private FilterDefinition<Step> CreateStepFilter(Guid stepId)
        {
            return Builders<Step>.Filter.Eq(s => s.Id, stepId);
        }
    }
}