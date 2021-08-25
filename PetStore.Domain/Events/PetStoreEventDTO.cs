using System;

namespace PetStore.Domain.Events
{
    public class PetStoreEventDTO
    {
        public readonly Guid ResourceID;
        public readonly string Name;
        public readonly string Type;

        public PetStoreEventDTO(Guid resourceID, string name, string type)
        {
            this.ResourceID = resourceID;
            this.Name = name;
            this.Type = type;
        }
    }
}