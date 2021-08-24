using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.Domain.Common
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model
    /// </summary>
    public interface IRepository<AggregateRoot, ErrorEnumType>
        where AggregateRoot : IAggregateRoot
        where ErrorEnumType : Enum
    {
        IUnitOfWork<ErrorEnumType> UnitOfWork { get; }


        // Access methods
        Task<bool> AddAsync(AggregateRoot entity, CancellationToken token = default(CancellationToken));
        Task<bool> AddRangeAsync(IEnumerable<AggregateRoot> entities, CancellationToken token = default(CancellationToken));
        Task<bool> DeleteAsync(AggregateRoot entity, CancellationToken token = default(CancellationToken));
        Task<bool> RemoveAsync(AggregateRoot entity, CancellationToken token = default(CancellationToken));
        Task<bool> UpdateAsync(AggregateRoot entity, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<AggregateRoot>> GetAllAsync(CancellationToken token = default(CancellationToken));
        Task<AggregateRoot> GetByResourceIDAsync(Guid resourceID, CancellationToken token = default(CancellationToken));

    }
}
