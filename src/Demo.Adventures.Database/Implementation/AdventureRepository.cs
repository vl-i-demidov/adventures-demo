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
    public class AdventureRepository : Repository<Adventure>, IAdventureRepository
    {
        static AdventureRepository()
        {
            BsonClassMap.RegisterClassMap<AdventureRepository>();
        }

        public AdventureRepository(IOptions<RepositoryConfig> config) :
            base(config, "adventure")
        {
        }

        public async Task<Adventure> GetAdventureAsync(Guid adventureId)
        {
            var filter = Builders<Adventure>.Filter.Eq(a => a.Id, adventureId);

            var cursor = await Collection.FindAsync(filter);
            var adventure = await cursor.FirstOrDefaultAsync();

            AssertExists<Adventure>(() => adventure != null, adventureId);

            return adventure;
        }

        public async Task CreateAdventureAsync(Adventure adventure)
        {
            await Collection.InsertOneAsync(adventure);
        }

        public async Task UpdateAdventureAsync(Guid adventureId, string title, Guid? firstStepId)
        {
            var filter = Builders<Adventure>.Filter.Eq(a => a.Id, adventureId);
            var update = Builders<Adventure>.Update;

            var definitions = new List<UpdateDefinition<Adventure>>();
            if (!string.IsNullOrEmpty(title)) definitions.Add(update.Set(a => a.Title, title));
            if (firstStepId != null) definitions.Add(update.Set(a => a.FirstStepId, firstStepId));

            var result = await Collection.UpdateOneAsync(filter, update.Combine(definitions));

            AssertExists<Adventure>(result, adventureId);
        }
    }
}