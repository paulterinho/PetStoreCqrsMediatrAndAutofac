using MediatR;
using Microsoft.Extensions.Logging;

namespace PetStore.API.Application.Query
{
    public partial class PetStoreQueryBase
    {

        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;

        public PetStoreQueryBase(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
    }
}
