using MediatR;
using Petstore.Common.Utils;
using PetStore.Domain.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSeedwork.Repositories
{
    /// <summary>
    /// Used to house the Domain Event Dispatching capabilities that will be shared across Service Repositories
    /// </summary>
    /// <typeparam name="ErrorEnumType"></typeparam>
    /// <typeparam name="EntityClass"></typeparam>
    public class AbstractDomainEventDispatcher<ErrorEnumType, EntityClass>
        where ErrorEnumType : Enum
        where EntityClass : Entity
    {
        protected readonly ILogger _logger;
        private readonly IMediator _mediator;

        public AbstractDomainEventDispatcher(ILogger logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected async Task DispatchDomainEventsOrThrowError(Result<ErrorEnumType> result, EntityClass obj)
        {
            if (result.IsFailure)
            {
                throw result.Error;
            }
            else
            {
                await _DispatchEvents(obj);
            }
        }

        protected async Task DispatchDomainEventsOrThrowError(Result<ErrorEnumType> result, IEnumerable<Entity> entities)
        {
            if (result.IsFailure)
            {
                throw result.Error;
            }
            else
            {
                await _DispatchEvents(entities);
            }
        }

        protected async Task _DispatchEvents(IEnumerable<Entity> entities)
        {
            try
            {
                // async/await doesn't work with .ForEach()
                foreach (var entity in entities)
                {
                    await _DispatchEvents(entity);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        protected async Task _DispatchEvents(Entity entity)
        {
            try
            {
                if ((entity != null) &&
                     (entity.DomainEvents != null) &&
                     (entity.DomainEvents.Count() > 0))
                {
                    // async/await doesn't work with .ForEach()
                    foreach (var domainEvent in entity.DomainEvents)
                    {
                        await _mediator.Publish(domainEvent);
                    }
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        protected void ClearDomainEvents(Entity entity)
        {
            try
            {
                if (entity != null && entity.DomainEvents != null)
                {
                    entity.ClearDomainEvents();
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }

        protected void ClearDomainEvents(IEnumerable<Entity> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    ClearDomainEvents(entity);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }
        }
    }
}
