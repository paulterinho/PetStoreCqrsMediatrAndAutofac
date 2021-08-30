using MediatR;
using Microsoft.EntityFrameworkCore;
using Petstore.Common.Utils;
using PetStore.Domain.Common;
using PetStore.Domain.Infrastructure.Common;
using PetStore.Domain.Infrastructure.Models;
using Serilog;
using ServiceSeedwork.Repositories;
using ServiceSeedwork.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.Domain.Infrastructure.Repositories
{
    /// <summary>
    /// An abstract class that lets us avoid having to copy-paste lots of code that does the same thing. 
    /// 
    /// To use this, make sure you extend it, and implement the abstract methods. 
    /// </summary>
    /// <typeparam name="DomainModel"></typeparam>
    /// <typeparam name="InfrastructrueModel"></typeparam>
    /// <typeparam name="ErrorEnum"></typeparam>
    /// <typeparam name="ContextInterface"></typeparam>
    public abstract class AbstractEntityRepository<DomainModel, InfrastructrueModel, ErrorEnum, ContextInterface> :

        // Extend the following elements
        AbstractDomainEventDispatcher<ErrorEnum, DomainModel>,
        IDomainInfrastructureMapper<DomainModel, InfrastructrueModel>,
        IGetMainDbSet<InfrastructrueModel>,
        IRepository<DomainModel, ErrorEnum>

        // Constrain the Generic Inputs
        where DomainModel : Entity, IAggregateRoot
        where ContextInterface : IUnitOfWork<ErrorEnum>
        where ErrorEnum : Enum
        where InfrastructrueModel : IDbEntity, ICreateModifyDeleteTimesUTC, IUpdatable<InfrastructrueModel>

    {

        public readonly ContextInterface _context;
        protected new readonly ILogger _logger;
        private readonly IMediator _mediator;

        public IUnitOfWork<ErrorEnum> UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// This constructor is used by Autofac. 
        /// </summary>
        public AbstractEntityRepository(ContextInterface context, ILogger logger, IMediator mediator) :
            base(logger, mediator)
        {
            _context = context; // ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        /// <summary>
        /// Abstract: Implement this method so that we can pass it a Domain Model, and it will give us back an Infrastructure Modelk
        /// </summary>
        public abstract InfrastructrueModel From(DomainModel domainModel);

        /// <summary>
        /// Abstract: Implement this method so that we can pass it a Domain Model, and it will give us back an Infrastructure Modelk
        /// </summary>
        public abstract DomainModel From(InfrastructrueModel infrastructureModel);

        /// <summary>
        /// Abstract: Implement this ASYNC method so we can get a hold of the main DB Set.
        /// 
        /// Make sure it is `Async`. 
        /// </summary>
        public abstract DbSet<InfrastructrueModel> GetMainDbSet();


        public async Task<bool> AddAsync(DomainModel domainModel, CancellationToken token = default(CancellationToken))
        {
            bool success = false;
            Result<ErrorEnum> result;

            try
            {
                InfrastructrueModel infraModel = From(domainModel);

                // Update the Times
                infraModel.CreatedDateTimeUTC = DateTime.UtcNow;
                infraModel.ModifiedDateTimeUTC = DateTime.UtcNow;

                // So this funky thing is a callback to the subclass. (It's calling itself!)
                DbSet<InfrastructrueModel> mainDbSet = GetMainDbSet();
                mainDbSet.Add(infraModel);

                result = await _context.SaveChangesResultAsync(token);

                await DispatchDomainEventsOrThrowError(result, domainModel);
                success = !result.IsFailure;
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            finally
            {
                ClearDomainEvents(domainModel);
            }

            return success;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<DomainModel> domainModels, CancellationToken token = default(CancellationToken))
        {
            bool success = false;
            List<InfrastructrueModel> converteddomainModels = new List<InfrastructrueModel>();
            Result<ErrorEnum> result;

            try
            {
                if (domainModels != null && domainModels.Count() > 0)
                {
                    foreach (var obj in domainModels)
                    {
                        converteddomainModels.Add(From(obj));
                    }

                    converteddomainModels.ForEach(w =>
                    {
                        w.CreatedDateTimeUTC = DateTime.UtcNow;
                        w.ModifiedDateTimeUTC = DateTime.UtcNow;
                    });

                    // So this funky thing is a callback to the subclass. (It's calling itself!)
                    GetMainDbSet().AddRange(converteddomainModels);

                    result = await _context.SaveChangesResultAsync(token);
                    await DispatchDomainEventsOrThrowError(result, domainModels);
                    success = !result.IsFailure;
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            finally
            {
                ClearDomainEvents(domainModels);
            }

            return success;
        }

        /// <summary>
        /// Marks a PetStore's Removed Date to the current time, but doesn't delete it's record. 
        /// </summary>
        public async Task<bool> RemoveAsync(DomainModel domainModel, CancellationToken token = default(CancellationToken))
        {
            bool success = false;
            Result<ErrorEnum> result;

            try
            {

                // So this funky thing, GetMainDbSet, is a callback to the subclass. (It's calling itself!)
                InfrastructrueModel domainModelInfrastructure = await GetMainDbSet()
                    .Where(w => w.ResourceID == domainModel.ResourceID)
                    .FirstOrDefaultAsync(token);

                domainModelInfrastructure.RemovedDateTimeUTC = DateTime.UtcNow;
                domainModelInfrastructure.ModifiedDateTimeUTC = DateTime.UtcNow;

                result = await _context.SaveChangesResultAsync(token);
                await DispatchDomainEventsOrThrowError(result, domainModel);
                success = !result.IsFailure;
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            finally
            {
                ClearDomainEvents(domainModel);
            }

            return success;
        }

        /// <summary>
        /// Removes the PetStore from the DB (doesn't just mark it's removed date)
        /// </summary>
        public async Task<bool> DeleteAsync(DomainModel domainModel, CancellationToken token = default(CancellationToken))
        {
            bool success = false;
            Result<ErrorEnum> result;

            try
            {
                // So this funky thing, GetMainDbSet, is a callback to the subclass. (It's calling itself!)
                InfrastructrueModel domainModelInfrastructure = await GetMainDbSet()
                    .Where(w => w.ResourceID == domainModel.ResourceID)
                    .FirstOrDefaultAsync(token);

                GetMainDbSet().Remove(domainModelInfrastructure);

                result = await _context.SaveChangesResultAsync(token);
                await DispatchDomainEventsOrThrowError(result, domainModel);
                success = !result.IsFailure;
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            finally
            {
                ClearDomainEvents(domainModel);
            }

            return success;
        }

        public async Task<bool> UpdateAsync(DomainModel domainModel, CancellationToken token = default(CancellationToken))
        {
            bool success = false;
            Result<ErrorEnum> result;

            try
            {
                DomainModel dModel = await GetByResourceIDAsync(domainModel.ResourceID, token);

                if (dModel != null)
                {
                    // So this funky thing, GetMainDbSet, is a callback to the subclass. (It's calling itself!)
                    InfrastructrueModel infrastructureModel = await GetMainDbSet()
                        .Where(w => w.ResourceID == domainModel.ResourceID)
                        .FirstOrDefaultAsync(token);

                    infrastructureModel.Update(From(domainModel));  // We can do this because we are saying the model must implement IUpdatable

                    infrastructureModel.ModifiedDateTimeUTC = DateTime.UtcNow;

                    result = await _context.SaveChangesResultAsync(token);
                    await DispatchDomainEventsOrThrowError(result, domainModel);
                    success = !result.IsFailure;
                }
            }
            catch (Exception exp)
            {
                // NOTE:    If you get a weird error about a "context being null", check the Application Log (via Output window), or the
                //          Elmah logs. It's probably an error with Mediatr Notification handlers. If a handler
                //          throws an error, it comes out as this generic nonsensical Exception. 
                //
                // @see https://stackoverflow.com/questions/32615330/handling-errors-exceptions-in-a-mediator-pipeline-using-cqrs
                //
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }
            finally
            {
                ClearDomainEvents(domainModel);
            }

            return success;
        }

        public async Task<IEnumerable<DomainModel>> GetAllAsync(CancellationToken token = default(CancellationToken))
        {
            List<DomainModel> returnPetStores = new List<DomainModel>();

            try
            {
                // So this funky thing, GetMainDbSet, is a callback to the subclass. (It's calling itself!)
                List<InfrastructrueModel> domainModels = await GetMainDbSet().ToListAsync(token);

                foreach (InfrastructrueModel w in domainModels)
                {
                    returnPetStores.Add(From(w));
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return returnPetStores;
        }

        public async Task<DomainModel> GetByResourceIDAsync(Guid resourceID, CancellationToken token = default(CancellationToken))
        {
            DomainModel returnPetStore = null;

            try
            {
                // So this funky thing, GetMainDbSet, is a callback to the subclass. (It's calling itself!)
                InfrastructrueModel infrastructureModel = await GetMainDbSet()
                  .Where(w => w.ResourceID == resourceID)
                  .FirstOrDefaultAsync(token);

                if (infrastructureModel != null)
                {
                    returnPetStore = From(infrastructureModel);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw;
            }

            return returnPetStore;
        }

    }
}
