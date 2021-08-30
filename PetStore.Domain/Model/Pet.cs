using Petstore.Common.Command;
using Petstore.Common.Utils;
using PetStore.Domain.Common;
using PetStore.Domain.Models;
using Serilog;
using System;

namespace PetStore.Domain.Model
{
    public partial class Pet : Entity, IAggregateRoot
    {

        public Pet()
        {
        }

        public Pet(Guid resourceID, string name, PetTypeEnum type =null)
        {
            ResourceID = resourceID;
            Name = name;
            Type = type;
        }

        public string Name { get; private set; }
        public PetTypeEnum? Type { get; private set; }


        public PetStoreDomainResponse _SetType(PetTypeEnum? type)
        {
            PetStoreDomainResponse response = new PetStoreDomainResponse();
            try
            {
                bool enumMissing = type == null;

                if (enumMissing)
                {
                    response.Errors.Add(nameof(Type), PetStoreErrorValue.Pet_Type_is_required);
                }

                if (response.Success)
                {
                    Type = type;
                }

            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return response;
        }

        public PetStoreDomainResponse _SetName(string name)
        {
            PetStoreDomainResponse response = new PetStoreDomainResponse();
            try
            {
                bool enumMissing = name == null;

                if (enumMissing)
                {
                    response.Errors.Add(nameof(Type), PetStoreErrorValue.Pet_Type_is_required);
                }

                if (response.Success)
                {
                    Name = name;
                }

            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return response;
        }

        public PetStoreDomainResponse _SetResourceID(Guid resourceID)
        {
            PetStoreDomainResponse response = new PetStoreDomainResponse();
            try
            {
                bool enumMissing = resourceID == default;

                if (enumMissing)
                {
                    response.Errors.Add(nameof(Type), PetStoreErrorValue.Pet_Resource_ID_is_required);
                }

                if (response.Success)
                {
                    ResourceID = resourceID;
                }

            }
            catch (Exception exp)
            {
                Log.Logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return response;
        }


    }
}
