using Autofac;
using Autofac.Integration.WebApi;
using MediatR;
using Microsoft.Extensions.Configuration;
using Petstore.Api.Application.Behavior;
using Petstore.API.Application.Validators;
using Petstore.Common.Config;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace Petstore.Api.Application.Config
{
    /// <summary>
    /// Class that will encapsulate the configuration for Mediatr and Autofac in the API project (And tests projects). 
    /// </summary>
    public class AutofacStart : AutofacAndMediatrLoader
    {
        HttpConfiguration _HttpConfig;
        IConfiguration _Configuration;

        public AutofacStart(IConfiguration configuration, ContainerBuilder builder = null, HttpConfiguration httpConfig = null) : base()
        {

            _HttpConfig = httpConfig;
            _Configuration = configuration;

            // Here are the modules we wish to load for the API project. 
            IEnumerable<Autofac.Module> modulesToLoad = new List<Autofac.Module>()
            {
                new AutofacMediatorModule(),
                new AutofacApplicationModule(_Configuration) // TODO: figure out how to log the correct environment for static usages of Logger. (Log.Logger.Error())
                
            };

            // Do the magic.
            Load(modulesToLoad, preConfig, postConfig, builder);
        }

        /// <summary>
        /// Callback that happens before we add the main depedencies ("modulesToLoad")
        /// </summary>
        private void preConfig(ContainerBuilder builder)
        {

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            if (_HttpConfig != null)
            {
                // OPTIONAL: Register the Autofac filter provider.
                builder.RegisterWebApiFilterProvider(_HttpConfig);
            }

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();
        }

        /// <summary>
        /// Callback that happens after we add the main depedencies ("modulesToLoad")
        /// </summary>
        private void postConfig(ContainerBuilder builder)
        {
            
            /*var contextOptions = new DbContextOptionsBuilder<PetStoreContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test")
                .Options;

            builder
                .RegisterType<PetStoreContext>()
                .WithParameter("options", _Configuration.GetValue<string>(PetStoreConstants.APP_SETTINGS_PROP_CONNECTION_STRING)) // get the configuration settings from  `appsettings.json`
                .InstancePerLifetimeScope(); */

            // Mediatr Request & Notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterGeneric(typeof(LoggingBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            // TODO: Uncomment this line when we want to do Command Validation BEFORE it gets to the domain layer
            //
             builder.RegisterGeneric(typeof(PetStoreValidatorPipelineBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            // TODO: Uncomment this line when we have cross Microservice Transactions to deal with.
            //
            //      builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

            // Misc
            builder.RegisterAssemblyTypes(typeof(IRequest).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan
        }
    }
}