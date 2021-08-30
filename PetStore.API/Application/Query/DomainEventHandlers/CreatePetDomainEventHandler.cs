using PetStore.Common.Utils;
using PetStore.Domain.Events;
using PetStores.API.Application.Queries.DomainEventHandlers;
using Serilog;

namespace PetStore.API.Application.Query.DomainEventHandlers
{
    /// <summary>
    /// Receives SaveDraftDomainEvent from the DB Context via Mediatr.
    /// </summary>
    public class CreatePetDomainEventHandler : CommonDomainEventHandler<CreatePetDomainEvent>
    {
        public CreatePetDomainEventHandler(ILogger logger, ISecretsManager waiverSecretsManager) :
            base(logger, waiverSecretsManager)
        { }
    }
}