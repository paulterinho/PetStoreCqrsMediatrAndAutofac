using MediatR;
using Microsoft.AspNetCore.Mvc;
using Petstore.Swagger.Io.Api.Application.Command;
using Petstore.Swagger.Io.Api.Application.Utils;
using Petstore.Swagger.Io.Common;
using Petstore.Swagger.Io.Common.Command;
using Serilog;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace PetStore.API.Application.Command
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api")]
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

        [Microsoft.AspNetCore.Mvc.HttpPost, Microsoft.AspNetCore.Mvc.Route("pets")]
        public override async Task<ActionResult<Pet>> CreatePet([FromBody] Pet pet, CancellationToken cancellationToken = default)
        {
            try
            {

                CreatePetCommand cmd = new CreatePetCommand(pet);
                Pet updatedPet = await _mediator.Send(cmd, cancellationToken);

                return Ok(updatedPet);
            }
            catch (PetStoreException exp)
            {
                return BadRequest(exp);
            }
            catch (Exception)
            {
                // it has already been logged, no need to re-log the exception
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
    }
}
