using MediatR;
using Microsoft.EntityFrameworkCore;
using Petstore.Swagger.Io.Common.Command;
using PetStore.Domain;
using PetStore.Domain.Infrastructure.Repositories;
using Serilog;
using DomainModels = PetStore.Domain.Model;
using InfraModels = PetStore.Infrastructure.Models;

namespace PetStore.Infrastructure.Repositories
{
    public class PetRepository :
        AbstractEntityRepository<DomainModels.Pet, InfraModels.Pet, PetStoreErrorValue, IPetStoreContext>,
        IPetRepository // NOTE: Using this interface as a marker interface for easier readability in the Autofac Module.  
    {

        public PetRepository(IPetStoreContext context, ILogger logger, IMediator mediator) :
            base(context, logger, mediator)
        {
            // all the magic happens in the base class.
        }

        public override InfraModels.Pet From(DomainModels.Pet domainModel)
        {
            return PetInfrastructureUtils.From(domainModel);
        }

        public override DomainModels.Pet From(InfraModels.Pet infrastructureModel)
        {
            return PetInfrastructureUtils.From(infrastructureModel);
        }

        public override DbSet<InfraModels.Pet> GetMainDbSet()
        {
            return _context.Pets;
        }
    }
}
