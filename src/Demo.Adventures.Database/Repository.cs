using System;
using Demo.Adventures.Common.Exceptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Demo.Adventures.Database
{
    public abstract class Repository<T>
    {
        protected readonly IMongoCollection<T> Collection;

        protected Repository(IOptions<RepositoryConfig> options, string collectionName)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.Database);
            Collection = db.GetCollection<T>(collectionName);
        }

        protected void AssertExists<TEntity>(UpdateResult updateResult, Guid id)
        {
            AssertExists<TEntity>(() => updateResult.ModifiedCount > 0, id);
        }

        protected void AssertExists<TEntity>(DeleteResult updateResult, Guid id)
        {
            AssertExists<TEntity>(() => updateResult.DeletedCount > 0, id);
        }

        protected void AssertExists<TEntity>(Func<bool> existsFunc, Guid id)
        {
            if (!existsFunc()) throw new EntityNotFoundException(id, typeof(T));
        }
    }
}