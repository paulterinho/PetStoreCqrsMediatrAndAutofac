using Autofac;
using Autofac.Integration.WebApi;
using MediatR;
using Petstore.Swagger.Io.Api.Application.Behavior;
using Petstore.Swagger.Io.Common.Config;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace Petstore.Swagger.Io.Api.Application.Config
{
    /// <summary>
    /// Class that will encapsulate the configuration for Mediatr and Autofac in the API project (And tests projects). 
    /// </summary>
    public class AutofacStart : AutofacAndMediatrLoader
    {
        HttpConfiguration Config;

        public AutofacStart(HttpConfiguration config) : base()
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            Config = config;

            // Here are the modules we wish to load for the API project. 
            IEnumerable<Autofac.Module> modulesToLoad = new List<Autofac.Module>()
            {
                new AutofacMediatorModule(),
                new AutofacApplicationModule("Incorrect Environment") // TODO: figure out how to log the correct environment for static usages of Logger. (Log.Logger.Error())
                
            };

            // Do the magic.
            Load(modulesToLoad, preConfig, postConfig);
        }

        /// <summary>
        /// Callback that happens before we add the main depedencies ("modulesToLoad")
        /// </summary>
        private void preConfig(ContainerBuilder builder)
        {

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(Config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();
        }

        /// <summary>
        /// Callback that happens after we add the main depedencies ("modulesToLoad")
        /// </summary>
        private IContainer postConfig(ContainerBuilder builder)
        {
            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            // TODO: Uncomment this line when we want to do Command Validation BEFORE it gets to the domain layer
            //
            //      builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            // TODO: Uncomment this line when we have cross Microservice Transactions to deal with.
            //
            //      builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

            // Misc
            builder.RegisterAssemblyTypes(typeof(IRequest).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            Config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }
    }
}