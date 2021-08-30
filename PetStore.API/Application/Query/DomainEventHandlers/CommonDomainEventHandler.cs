using Dapper;
using MediatR;
using Petstore.Common.Utils;
using PetStore.Common.Utils;
using PetStore.Domain.Events;
using Serilog;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace PetStores.API.Application.Queries.DomainEventHandlers
{
    /// <summary>
    /// Receives PetStoreStartedDomainEvents from the DB Context via Mediatr.
    /// </summary>
    public class CommonDomainEventHandler<T> : INotificationHandler<T>
        where T : PetStoreDomainEvent
    {
        private readonly string _connectionString = string.Empty;
        protected readonly ILogger _logger;

        public CommonDomainEventHandler(ILogger logger, ISecretsManager SecretsManager)
        {
            try
            {
                this._connectionString = SecretsManager.GetDbConnectionString();
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT);
                throw;
            }
        }

        public async Task Insert(T notification, CancellationToken cancellationToken)
        {
            try
            {

                // TODO: Get AutoMapper working
                object dto = new
                {
                    ResourceID = notification.PetStoreDTO.ResourceID,
                    Type = notification.PetStoreDTO.Type,
                    Name = notification.PetStoreDTO.Name,
                };



                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    var sqlStatement = @"
                         INSERT INTO petQuery.Pet
                            (
                                [ResourceID],
                                [Type],
                                [Name]
                            )
                         VALUES
                            (
                                @ResourceID,
                                @Name,
                                @Type
                            )
                        ";

                    // TODO: Should we use an anonymous object to make sure the names maatch the query. 
                    await connection.ExecuteAsync(sqlStatement, dto);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT);
                throw;
            }
        }


        private async Task Update(T notification, CancellationToken cancellationToken)
        {
            try
            {

                // TODO: Get AutoMapper working
                object dto = new
                {
                    ResourceID = notification.PetStoreDTO.ResourceID,
                    Name = notification.PetStoreDTO.Name,
                    Type = notification.PetStoreDTO.Type
                };

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    var sqlStatement = @"
                           UPDATE petQuery.Pet
                           SET
                                [PetStoreID] = @PetStoreID,  
                                [Name] = @Name,  
                                [Type] = @Type
                            WHERE 
                               ResourceID = @ResourceID";

                    await connection.ExecuteAsync(sqlStatement, dto);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT);
                throw;
            }
        }

        /// <summary>
        /// Main Entry point, it will figure out if the ResourceID exists or not and delegate to the Update or Insert methods.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(T notification, CancellationToken cancellationToken)

        {
            try
            {
                bool alreadyExists = false;

                // TOOD: see why this is notification handler triggering twice for one event.
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    var sqlStatement = @"SELECT 1 WHERE EXISTS (SELECT 1 FROM petQuery.Pet WHERE ResourceID = @ResourceID)";
                    alreadyExists = await connection.ExecuteScalarAsync<bool>(sqlStatement, new { ResourceID = notification.PetStoreDTO.ResourceID });
                }

                if (alreadyExists == true)
                {
                    await Update(notification, cancellationToken);
                }
                else
                {
                    await Insert(notification, cancellationToken);
                }
            }
            catch (Exception exp)
            {
                _logger.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT);
                throw;
            }
        }
    }
}