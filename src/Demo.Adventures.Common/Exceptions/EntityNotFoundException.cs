using System;
using System.Net;

namespace Demo.Adventures.Common.Exceptions
{
    /// <summary>
    ///     The exception that is thrown when the requested entity not found.
    /// </summary>
    public class EntityNotFoundException : ServiceException
    {
        public EntityNotFoundException(Guid id, Type entityType) :
            base($"{entityType.Name} not found. Id: {id}", HttpStatusCode.NotFound)
        {
        }

        public EntityNotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {
        }
    }
}