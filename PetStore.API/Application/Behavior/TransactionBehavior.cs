using MediatR;
//using Microsoft.Extensions.Logging;
//using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

// using Waivers.API.Application.IntegrationEvents;
//using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;
//using Microsoft.eShopOnContainers.Services.Waivers.Infrastructure;

namespace Petstore.Swagger.Io.Api.Application.Behavior
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        // TODO: throw this away or adopt it. It's mostly integrated. 
        /*
        
        private readonly ILogger _logger;
        private readonly WaiversContext _dbContext;
        private readonly IWaiversIntegrationEventService _waiversIntegrationEventService;

        public TransactionBehaviour(WaiversContext dbContext,
            IWaiversIntegrationEventService orderingIntegrationEventService,
            ILogger logger)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(WaiversContext));
            _waiversIntegrationEventService = orderingIntegrationEventService ?? throw new ArgumentException(nameof(orderingIntegrationEventService));
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            
            var response = default(TResponse);
            var typeName = typeof(TRequest); // TODO: did we need this line from the eShop src? // request.GetGenericTypeName();request.GetGenericTypeName();

            try
            {
                if (_dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.ExecuteAsync(async () =>
                {
                    Guid transactionId;

                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                        response = await next();

                        _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                        await _dbContext.CommitTransactionAsync(transaction);

                        transactionId = transaction.TransactionId;
                    }

                    await _waiversIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }*/

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            throw new Exception("Transaction behavior not implemented yet.");
        }
    }
}
