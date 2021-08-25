using PetStore.Domain.Infrastructure.Common;

namespace PetStore.Domain.Infrastructure.Models
{
    public interface IUpdatable<InfrastructureModel>

        where InfrastructureModel : IDbEntity
    {
        void Update(InfrastructureModel updatedPetStore);
    }
}
