using Autofac;
using AutofacSerilogIntegration;
using Dapper;
using Microsoft.Extensions.Configuration;
using PetStore.API.Application.Query.DB;
using PetStore.Common.Utils;
using PetStore.Domain;
using PetStore.Infrastructure;
using PetStore.Infrastructure.Repositories;
using Serilog;
using System;

namespace Petstore.Api.Application.Config
{
    public class AutofacApplicationModule : Autofac.Module
    {
        private IConfiguration Configuration;

        public AutofacApplicationModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Dapper needs some interpretation. These lines help the library to know what to do with different types.
            // 
            // For more info on creating custom mappers: https://medium.com/dapper-net/custom-type-handling-4b447b97c620
            //
            // Also note you can't make Type Handlers for Enums to get their EnumMember.Value attributes. It's a known issue. 
            //
            SqlMapper.AddTypeHandler(new DapperGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            /*  NOTE:   There are two ways to configure Serilog, via instance, and via static instance. The Static 
             *          instance is said to integrate better with autofac. 
             *          
             *          By assigning the newly configured logger to the static instance, we can also enable static methods to use the logger.
             *          
             *          @see https://github.com/nblumhardt/autofac-serilog-integration
             */
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings() //  NOTE A:     This reads the settings from Web.config in the project root.
                .WriteTo.Console() //       NOTE B: 1)  This tells it to write to the "Immediate Window in VS Studio"
                                   //                   (@see https://github.com/serilog/serilog/wiki/Formatting-Output#formatting-plain-text )
                                   //               2)  The "outputFormat" is specified in Web.config (mentioned in note A)
                .CreateLogger();

            // Entity Framework Context
            builder
              .RegisterType<PetStoreContext>()
              .As<IPetStoreContext>()
              .InstancePerLifetimeScope();

            // Entity Framework Repository
            builder.RegisterType<PetRepository>()
               .As<IPetRepository>()
               .InstancePerLifetimeScope();
            
            // CQRS Queries Repository.
            builder.RegisterType<PetStoreQueriesRepository>()
               .As<IPetStoreQueriesRepository>()
               .InstancePerLifetimeScope();

            // Entity Framework Repository
            builder.RegisterType<SecretsManager>()
               .As<ISecretsManager>()
               .InstancePerLifetimeScope();

            builder.RegisterLogger(); //If no logger is explicitly passed to this function, the default Log.Logger will be used.
        }
    }
}