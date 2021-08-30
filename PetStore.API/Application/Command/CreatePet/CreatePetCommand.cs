using MediatR;
using Petstore.Common.Command;

namespace Petstore.Api.Application.Command
{
    public class CreatePetCommand : IRequest<Pet>
    {
        public Pet Pet { get; }

        public CreatePetCommand(Pet pet)
        {
            Pet = pet;
        }
    }
}