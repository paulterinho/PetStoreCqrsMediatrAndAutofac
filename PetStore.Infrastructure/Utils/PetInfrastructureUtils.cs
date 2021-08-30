using Petstore.Common.Utils;
using Serilog;
using System;
using DomainModels = PetStore.Domain.Model;
using InfraModels = PetStore.Infrastructure.Models;

namespace PetStore.Infrastructure
{
    /**
     * Used to convert Infrastructure Models to Domain Models and Vice-Versa. 
     */
    public class PetInfrastructureUtils
    {
        /// <summary>
        /// Convert from a Domain Pet to an Infrastructure Pet.
        /// </summary>
        public static InfraModels.Pet From(DomainModels.Pet pet)
        {
            InfraModels.Pet returnPet = null;

            try
            {



                returnPet = new InfraModels.Pet()
                {

                    Name = pet.Name,
                    ResourceID = pet.ResourceID,
                    Type = pet.Type.ToString(),
                };
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return returnPet;
        }

        /// <summary>
        /// Convert from an Infrastructure Pet to a Domain Pet.
        /// </summary>
        public static DomainModels.Pet From(InfraModels.Pet pet)
        {
            DomainModels.Pet returnPet = null;


            try
            {
                returnPet = new DomainModels.Pet(
                    pet.ResourceID,
                    pet.Name,
                    (pet.Type != null) ? DomainModels.PetTypeEnum.FromName(pet.Type) : null
              );
            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return returnPet;
        }
    }
}