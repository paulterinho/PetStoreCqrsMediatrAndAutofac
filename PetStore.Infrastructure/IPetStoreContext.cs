﻿using Microsoft.EntityFrameworkCore;
using PetStore.Domain.Common;
using PetStore.Infrastructure.Models;
using SDK = Petstore.Common.Command;

namespace PetStore.Infrastructure
{
    public interface IPetStoreContext : IUnitOfWork<SDK.PetStoreErrorValue>
    {

        DbSet<Pet> Pets { get; set; }

    }
}