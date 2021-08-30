using Microsoft.AspNetCore.Mvc.ModelBinding;
using Petstore.Common.Command;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.API.Application.Query.DB
{
    public interface IPetStoreQueriesRepository
    {
        Task<PetCollection> ListPets(
            int? limit,
            int? offset,
            IEnumerable<PetSortValue> sorts = null,
            IEnumerable<string> namesToFilter = null,
            IEnumerable<PetTypeValue> typesToFilter = null,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<Pet> ShowPetById([BindRequired] string petId,
            CancellationToken cancellationToken = default);
    }
}