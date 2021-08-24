using MediatR;
using Microsoft.AspNetCore.Mvc;
using Petstore.Swagger.Io.Common.Command;
using Serilog;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.API.Application.Command
{
    public class PetStoreCommandController : CommandControllerBase
    {

        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;

        /// <summary>
        /// This constructor is for Autofac
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public PetStoreCommandController(IMediator mediator, ILogger logger) : base()
        {
            _mediator = mediator;
            _logger = logger;
        }

        public override Task<HttpResponseMessage> CreatePet([FromBody] Pet body, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
