using FluentValidation;
using MediatR;
using Petstore.Swagger.Io.Common.Utils;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// NOTE: keep these usings in here to see the context how of it's used in the github eShops example.
//
//      using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
//      using Microsoft.Extensions.Logging;
//      using Ordering.Domain.Exceptions;

namespace Petstore.Swagger.Io.Api.Application.Behavior
{



    /// <summary>
    ///     <para>
    ///         Message/Command interceptor: So this class is part of the messaging pipeline. Anytime you send a Command, 
    ///         this will intercept the command msg and take action.
    ///     <para>
    ///     <para>
    ///         This particular class is only responsibile for finding other validators registered via AutoFac (See WaiverMediatorModule.cs)
    ///     </para>
    ///     <para>
    ///         See the eShops example for more information: https://github.com/dotnet-architecture/eShopOnContainers 
    ///     </para>
    /// </summary>
    /// <typeparam name="TRequest">This will be the Domain CommandEvent type</typeparam>
    /// <typeparam name="TResponse">This will be the same as the Domain Event Handler in the same usecase.</typeparam>
    /// <typeparam name="ExceptionType">This identifies the exception type that will be thrown</typeparam>
    /// <typeparam name="ErrorEnumType">This is the Error Enum to be used when throwing an Exception</typeparam>
    public abstract class ValidatorBehavior<TRequest, TResponse, ExceptionType, ErrorEnumType> : IPipelineBehavior<TRequest, TResponse>
        where ExceptionType : ResultException<ErrorEnumType>
        where ErrorEnumType : Enum
    {
        protected readonly ILogger _logger;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehavior(IValidator<TRequest>[] validators, ILogger logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                var typeName = typeof(TRequest); // TODO: did we need this line from the eShop src? // request.GetGenericTypeName();

                _logger.Information("----- Validating command {CommandType}", typeName);

                // NOTE: This will look for validators registered witht he <TRequest> type (If it's a SaveWaiverDraftCommand Validator),
                //       it will get added via Autofac to this class.
                var failures = _validators
                    .Select(v => v.Validate(request))
                    .SelectMany(result => result.Errors)
                    .Where(error => error != null)
                    .ToList();

                // Log them errors yo!
                if (failures.Any())
                {
                    _logger.Error("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

                    // SECRET SAUCE: we are calling the sub class's implementation of this method. 
                    throw CreateException(failures);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT, exp.Message);
                throw exp;
            }

            return await next();
        }


        /// <summary>
        /// This method will construct a specific Exception type so we can avoid using Reflection. (Easier to read and deal with)
        /// </summary>
        /// <returns></returns>
        protected abstract ExceptionType CreateException(System.Collections.Generic.List<FluentValidation.Results.ValidationFailure> failures);

    }
}