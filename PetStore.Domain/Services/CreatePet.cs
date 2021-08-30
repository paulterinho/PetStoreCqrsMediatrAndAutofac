using Petstore.Common.Command;
using PetStore.Domain.Common;
using PetStore.Domain.Events;
using PetStore.Domain.Models;

namespace PetStore.Domain.Model
{

    public partial class Pet
        : Entity, IAggregateRoot
    {
        public PetStoreDomainResponse CreatePet(Pet pet)
        {
            PetStoreDomainResponse response = new PetStoreDomainResponse();

            bool resourceIDsDoNotMatch = ResourceID != pet.ResourceID;

            if (resourceIDsDoNotMatch)
            {
                response.Errors.Add(nameof(ResourceID), PetStoreErrorValue.Pet_Resource_IDs_do_not_match);
            }

            PetStoreDomainResponse setName = _SetName(pet.Name);
            response.AddErrors(setName.Errors);

            PetStoreDomainResponse setPubStat = _SetType(pet.Type);

            // Add an event to potentially dispatch later.
            AddSaveDomainEvent(this);

            return response;
        }

        /// <summary>
        /// Dispatch a domain event so the change log can be updated (or the Query side of CQRS if that is ever implemented.)
        /// </summary>
        private void AddSaveDomainEvent(Pet pet)
        {
            CreatePetDomainEvent createPetDomainEvt = new CreatePetDomainEvent(
                pet.ResourceID,
                pet.Name,
                pet.Type.ToString()
            );

            AddDomainEvent(createPetDomainEvt);
        }
    }
}
