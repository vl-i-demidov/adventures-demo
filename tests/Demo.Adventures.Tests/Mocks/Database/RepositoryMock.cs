using System;
using System.Collections.Concurrent;
using Demo.Adventures.Common.Exceptions;

namespace Demo.Adventures.Tests.Mocks.Database
{
    internal abstract class RepositoryMock<T>
    {
        protected ConcurrentDictionary<Guid, T> Collection { get; }

        protected RepositoryMock()
        {
            Collection = new ConcurrentDictionary<Guid, T>();
        }

        protected void AddEntity(Guid id, T entity)
        {
            // todo: check if entity already exists
            Collection.TryAdd(id, entity);
        }

        protected T GetEntity(Guid id)
        {
            if (Collection.TryGetValue(id, out var entity)) return entity;

            throw GetEntityNotFoundException<T>(id);
        }

        protected void DeleteEntity(Guid id)
        {
            // assert exists
            GetEntity(id);

            Collection.TryRemove(id, out _);
        }

        protected EntityNotFoundException GetEntityNotFoundException<TEntity>(Guid id)
        {
            return new EntityNotFoundException(id, typeof(TEntity));
        }
    }
}