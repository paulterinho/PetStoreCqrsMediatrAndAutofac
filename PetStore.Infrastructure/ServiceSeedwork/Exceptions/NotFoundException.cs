using System;

namespace PetStore.Domain.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for repo exceptions
    /// </summary>
    public class NotFoundException
        : Exception
    {
        public NotFoundException(string message = null)
            : base(message)
        { }
    }
}