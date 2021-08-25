using PetStore.Domain.Common;
using PetStore.Domain.Infrastructure.Common;

namespace PetStore.Domain.Infrastructure
{
    /// <summary>
    /// Generic interface to indicate mapping from a Infrastructructure Model to a Domain and vice-versa.
    /// </summary>
    /// <typeparam name="DomainModel"></typeparam>
    /// <typeparam name="InfrastructrueModel"></typeparam>
    public interface IDomainInfrastructureMapper<DomainModel, InfrastructrueModel>
        where DomainModel: Entity
        where InfrastructrueModel: IDbEntity
    {
        DomainModel From(InfrastructrueModel infrastructureModel);

        InfrastructrueModel From(DomainModel domainModel);
    }
}