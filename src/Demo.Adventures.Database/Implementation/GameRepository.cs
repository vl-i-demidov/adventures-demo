using System;
using System.Threading.Tasks;
using Demo.Adventures.Database.Contracts;
using Demo.Adventures.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Demo.Adventures.Database.Implementation
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        static GameRepository()
        {
            BsonClassMap.RegisterClassMap<GameRepository>();
        }

        public GameRepository(IOptions<RepositoryConfig> config) :
            base(config, "game")
        {
        }

        public async Task<Game> GetGameAsync(Guid gameId)
        {
            var filter = Builders<Game>.Filter.Eq(s => s.Id, gameId);

            var gameCursor = await Collection.FindAsync(filter);
            var game = await gameCursor.FirstOrDefaultAsync();

            AssertExists<Game>(() => game != null, gameId);

            return game;
        }

        public async Task CreateGameAsync(Game game)
        {
            await Collection.InsertOneAsync(game);
        }

        public async Task AddSelectedOptionAsync(Guid gameId, SelectedOption option)
        {
            var filter = CreateGameFilter(gameId);
            var update = Builders<Game>.Update.Push(s => s.SelectedOptions, option);

            var result = await Collection.UpdateOneAsync(filter, update);

            AssertExists<Game>(result, gameId);
        }

        private FilterDefinition<Game> CreateGameFilter(Guid gameId)
        {
            return Builders<Game>.Filter.Eq(g => g.Id, gameId);
        }
    }
}