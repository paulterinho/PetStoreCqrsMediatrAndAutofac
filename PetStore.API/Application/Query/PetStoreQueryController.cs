using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Petstore.Common;
using Petstore.Common.Command;
using PetStore.API.Application.Query.DB;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.API.Application.Query
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api")]
    public class PetStoreQueryController : QueryControllerBase
    {

        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;
        protected readonly IPetStoreQueriesRepository _petQueriesRepo;

        /// <summary>
        /// This constructor is for Autofac
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public PetStoreQueryController(IMediator mediator, ILogger logger, IPetStoreQueriesRepository petQueriesRepo) : base()
        {
            _mediator = mediator;
            _logger = logger;
            _petQueriesRepo = petQueriesRepo;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("pets")]
        public override async Task<ActionResult<PetCollection>> ListPets(
            [FromQuery] int? limit,
            [FromQuery] int? offset,
            [FromQuery] IEnumerable<PetSortValue> sorts,
            [FromQuery] IEnumerable<string> namesToFilterBy,
            [FromQuery] IEnumerable<string> typesToFilterBy,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                PetCollection petCollection = await _petQueriesRepo.ListPets(limit, offset, sorts, namesToFilterBy, typesToFilterBy, cancellationToken);
                return Ok(petCollection);
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

        [Microsoft.AspNetCore.Mvc.HttpGet, Microsoft.AspNetCore.Mvc.Route("pets/{petId}")]
        public override async Task<ActionResult<Pet>> ShowPetById([BindRequired] string petId, CancellationToken cancellationToken = default)
        {
            try
            {
                Pet pet = await _petQueriesRepo.ShowPetById(petId, cancellationToken);
                return Ok(pet);
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
