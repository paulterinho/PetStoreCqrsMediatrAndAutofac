using MediatR;
using Petstore.Swagger.Io.Common.Command;

namespace Petstore.Swagger.Io.Api.Application.Command
{
    public class CreatePetCommand : IRequest<bool>
    {
        public Pet Pet { get; }

        public CreatePetCommand(Pet pet)
        {
            Pet = pet;
        }
    }
}