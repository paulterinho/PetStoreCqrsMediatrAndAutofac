using Microsoft.EntityFrameworkCore;
using Petstore.Common;
using Petstore.Common.Command;
using PetStore.Common.Utils;
using PetStore.Domain.Infrastructure.Services;
using Serilog;
using System;
using System.Collections.Generic;
using InfraModel = PetStore.Infrastructure.Models;
using SDK = Petstore.Common.Command;

namespace PetStore.Infrastructure
{
    public class PetStoreContext :
        AbstractContext<InfraModel.Pet, PetStoreErrorValue, PetStoreContext, PetStoreException>,
        IPetStoreContext // NOTE: this is just a marker for the Autofac module. It makes it easier to read.
    {
        public PetStoreContext(ILogger logger, ISecretsManager petSecretsManager) :
            base(logger, petSecretsManager)
        {
            // Magic happening in the base class.
        }


        protected override PetStoreException HandleException(Exception exception)
        {
            PetStoreException exp = null;

            // find the innermost exception
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            if (exception.Message.Contains("idx_pet_id_notnull"))
            {
                exp = new PetStoreException(SDK.PetStoreErrorValue.Pet_ID_is_not_unique,
                    new Dictionary<string, SDK.PetStoreErrorValue>(),
                    SDK.PetStoreErrorValue.Pet_ID_is_not_unique.ToString());

                exp.Errors.Add(nameof(InfraModel.Pet.ID), SDK.PetStoreErrorValue.Pet_ID_is_not_unique);
            }

            return exp;
        }

        public virtual DbSet<InfraModel.Pet> Pets { get; set; }

    }
}

