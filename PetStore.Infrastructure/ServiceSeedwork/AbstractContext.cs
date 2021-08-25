using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Petstore.Swagger.Io.Common.Utils;
using PetStore.Domain.Common;
using PetStore.Common.Utils;
using PetStore.Domain.Infrastructure.Common;
using PetStore.Domain.Infrastructure.Models;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PetStore.Domain.Infrastructure.Services
{
    public abstract class AbstractContext<InfrastructureModel, ErrorEnumType, DbContextClass, ExceptionClass> : DbContext
        where InfrastructureModel:  IDbEntity,
                                    ICreateModifyDeleteTimesUTC, // makes us use dates for updating / removing / deleting
                                    IUpdatable<InfrastructureModel>
        where ErrorEnumType : Enum  // What Error Enums we should use
        where DbContextClass : DbContext // Which DB context this is (It should be the base class using this class)
        where ExceptionClass : ResultException<ErrorEnumType> // The Exception you wish to throw. 
    {
        
        private readonly ILogger _logger;

        public AbstractContext(ILogger logger, SecretsManager secretsManager, string host = null) :
            base(secretsManager.GetDbConnectionString(host ?? HttpContext.Current.Request.Url.Host))
        {
            //disable code-first
            //Database.SetInitializer<DbContextClass>(null);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public AbstractContext()
        {
            //disable code-first
            //Database.SetInitializer<DbContextClass>(null);
        }

        public async Task<Result<ErrorEnumType>> SaveChangesResultAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                int rowsAffected = await this.SaveChangesAsync(cancellationToken);
                return await Result<ErrorEnumType>.Success(rowsAffected);
            }
            catch (Exception exp)
            {
                _logger?.Error(exp, PetStoreConstants.ERROR_LOGGING_FORMAT);
                ExceptionClass error = HandleException(exp);

                // if we can't find an error, throw the actual db
                if (error == null)
                {
                    throw exp;
                }

                return await Result<ErrorEnumType>.Failure(error);
            }
        }

        /// <summary>
        /// This method to be implemented in the sub-class bc it will have logic we can't make generic.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected abstract ExceptionClass HandleException(Exception exception);


        // -------------------------------
        // EXAMPLE OF HOW TO IMPLEMENT
        // ------------------------------
        /*{
            ExceptionClass exp = null;

            // find the innermost exception
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            if (exception.Message.Contains("idx_object_id_notnull"))
            {
                exp = new ExceptionClass(ErrorEnumType.PetStore_ID_is_not_unique, new SDK.PetStoreErrors(), ErrorEnumType.PetStore_ID_is_not_unique.ToString());
                exp.Errors.Add(nameof(PetStore.PetStoreID), ErrorEnumType.PetStore_ID_is_not_unique);
            }

            return exp;
        }*/


    }
}
