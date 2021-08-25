using System;

namespace PetStore.Domain.Events
{
    public sealed class CreatePetDomainEvent : PetStoreDomainEvent
    {
        public CreatePetDomainEvent(Guid resourceID, string name, string type) : base(resourceID, name, type)
        {
        }
    }
}
