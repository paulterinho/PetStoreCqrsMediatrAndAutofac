using MediatR;
using Petstore.Swagger.Io.Api.Application.Utils;
using Petstore.Swagger.Io.Common;
using Petstore.Swagger.Io.Common.Command;
using Petstore.Swagger.Io.Common.Utils;
using PetStore.Domain;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using DomainModels = PetStore.Domain.Model;

namespace Petstore.Swagger.Io.Api.Application.Command
{
    public class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, bool>
    {
        private readonly IPetRepository _petRepository;
        private readonly ILogger _logger;

        public CreatePetCommandHandler(IPetRepository petRepository, ILogger logger)
        {
            _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //TODO: we need an identity service...
        }

        public async Task<bool> Handle(CreatePetCommand command, CancellationToken cancellationToken)
        {
            bool success = false;

            try
            {
                DomainModels.Pet changedPet = PetStoreApiUtils.From(command.Pet);

                //todo we need to get my resource id when editing a pet but not when transitioning from "unsaved" to "draft"
                DomainModels.Pet pet = await _petRepository.GetByResourceIDAsync(changedPet.ResourceID, cancellationToken);

                if (pet != null)
                {
                    pet.CreatePet(changedPet);

                    success = await _petRepository.UpdateAsync(pet, cancellationToken);
                }
                else
                {
                    throw new PetStoreException(PetStoreErrorValue.Pet_cannot_be_null);
                }
            }
            catch (PetStoreException exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return success;
        }
    }
}