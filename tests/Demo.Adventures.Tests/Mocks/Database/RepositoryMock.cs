using System;
using System.Collections.Concurrent;
using Demo.Adventures.Common.Exceptions;

namespace Demo.Adventures.Tests.Mocks.Database
{
    internal abstract class RepositoryMock<T>
    {
        protected readonly ConcurrentDictionary<Guid, T> _collection;

        protected RepositoryMock()
        {
            _collection = new ConcurrentDictionary<Guid, T>();
        }

        protected void AddEntity(Guid id, T entity)
        {
            // todo: check if entity already exists
            _collection.TryAdd(id, entity);
        }

        protected T GetEntity(Guid id)
        {
            if (_collection.TryGetValue(id, out var entity)) return entity;

            throw GetEntityNotFoundException<T>(id);
        }

        protected void DeleteEntity(Guid id)
        {
            // assert exists
            GetEntity(id);

            _collection.TryRemove(id, out _);
        }

        protected EntityNotFoundException GetEntityNotFoundException<TEntity>(Guid id)
        {
            return new EntityNotFoundException(id, typeof(TEntity));
        }
    }
}