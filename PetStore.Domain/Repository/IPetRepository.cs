using SDK = Petstore.Common.Command;
using PetStore.Domain.Common;
using PetStore.Domain.Model;

namespace PetStore.Domain
{
    public interface IPetRepository : IRepository<Pet, SDK.PetStoreErrorValue>
    {
    }
}
