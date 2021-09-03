using Petstore.Common.Utils;
using PetStore.Domain.Model;
using Serilog;
using System;
using DomainModels = PetStore.Domain.Model;
using SDK = Petstore.Common.Command;

namespace Petstore.Api.Application.Utils
{
    public class PetStoreApiUtils
    {
        public static DomainModels.Pet From(SDK.Pet pet)
        {
            DomainModels.Pet domainModel = null;
            try
            {
                domainModel = new DomainModels.Pet(
                    pet.ResourceID, pet.Name,
                    (pet.Type != null) ? PetTypeEnum.FromName(pet.Type?.ToString()) : null); // can be null
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return domainModel;
        }


        public static SDK.Pet From(DomainModels.Pet changedPet)
        {
            SDK.Pet sdkModel = null;
            try
            {
                sdkModel = new SDK.Pet()
                {
                    ResourceID = changedPet.ResourceID,
                    Name = changedPet.Name,
                    Type = changedPet.Type.EnumValue
                };
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return sdkModel;
        }


    }
}