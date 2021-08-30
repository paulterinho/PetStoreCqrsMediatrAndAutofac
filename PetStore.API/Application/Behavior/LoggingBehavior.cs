using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

// NOTE: keep these usings in here to see the context how of it's used in the github eShops example.
//
//       using Microsoft.Extensions.Logging;
//       using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Extensions;

namespace Petstore.Api.Application.Behavior
{
    /// <summary>
    /// So this class is part of the messaging pipeline. Anytime you send a Command, 
    /// this will intercept the command msg and take action.
    /// 
    /// <para>This particular class is only responsibile for logging information.</para>
    /// 
    ///<para>See the eShops example for more information: https://github.com/dotnet-architecture/eShopOnContainers</para>
    /// </summary>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;
        public LoggingBehavior(ILogger logger) => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var typeName = typeof(TRequest); // TODO: did we need this line from the eShop src? // request.GetGenericTypeName();

            _logger.Information("----- Handling command {CommandName} ({@Command})", typeName, request);
            var response = await next();
            _logger.Information("----- Command {CommandName} handled - response: {@Response}", typeName, response);

            return response;
        }
    }
}
