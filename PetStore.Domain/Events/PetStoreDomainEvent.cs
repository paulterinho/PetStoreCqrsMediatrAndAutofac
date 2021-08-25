using MediatR;
using System;

namespace PetStore.Domain.Events
{
    public class PetStoreDomainEvent : INotification
    {
        public readonly PetStoreEventDTO PetStoreDTO;

        public PetStoreDomainEvent(
            Guid resourceID,
            string name,
            string type)
        {
            PetStoreDTO = new PetStoreEventDTO(
                resourceID,
                name,
                type);            
        }
    }
}