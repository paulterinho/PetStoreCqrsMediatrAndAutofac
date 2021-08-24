using PetStore.Domain.Common;
using System;

namespace PetStore.Domain.Model
{
    public class Pet : Entity, IAggregateRoot
    {
        public void CreatePet(Pet changedPet)
        {
            throw new NotImplementedException();
        }
    }
}
