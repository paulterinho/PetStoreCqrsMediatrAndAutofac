using MediatR;
using Petstore.Api.Application.Utils;
using Petstore.Common;
using Petstore.Common.Command;
using Petstore.Common.Utils;
using PetStore.Domain;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using DomainModels = PetStore.Domain.Model;

namespace Petstore.Api.Application.Command
{
    public class CreatePetCommandHandler : IRequestHandler<CreatePetCommand, Pet>
    {
        private readonly IPetRepository _petRepository;
        private readonly ILogger _logger;

        public CreatePetCommandHandler(IPetRepository petRepository, ILogger logger)
        {
            _petRepository = petRepository ?? throw new ArgumentNullException(nameof(petRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Pet> Handle(CreatePetCommand command, CancellationToken cancellationToken)
        {
            Pet pet = null;
            bool success = false;
            DomainModels.Pet newPet = null;
            DomainModels.Pet existingPet = null;

            try
            {
                newPet = PetStoreApiUtils.From(command.Pet);

                existingPet = new DomainModels.Pet();

                existingPet.CreatePet(newPet);

                success = await _petRepository.AddAsync(existingPet);

                if (success == false)
                {
                    throw new Exception("Unable to save to the database.");
                }

                // convert it back to an API object
                pet = PetStoreApiUtils.From(existingPet);

            }
            catch (PetStoreException exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return pet;
        }
    }
}