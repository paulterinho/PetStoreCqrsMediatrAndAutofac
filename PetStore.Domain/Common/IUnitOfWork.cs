using System;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.Domain.Common
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model
    /// </summary>
    public interface IUnitOfWork<ErrorEnumType> : IDisposable
        where ErrorEnumType : Enum
    {
        /// <summary>
        /// Saves to the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>An int that represents the number of DB tuples affected</returns>
        Task<Result<ErrorEnumType>> SaveChangesResultAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}