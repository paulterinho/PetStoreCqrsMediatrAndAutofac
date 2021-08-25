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

            try
            {
                DomainModels.Pet changedPet = PetStoreApiUtils.From(command.Pet);

                success = await _petRepository.AddAsync(changedPet);

                if (success == false)
                {
                    throw new Exception("Unable to save to the database.");
                }

                pet = PetStoreApiUtils.From(changedPet);

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

            return pet;
        }
    }
}