using Autofac;
using FluentValidation;
using MediatR;
using Petstore.Swagger.Io.Api.Application.Command;
using PetStore.API.Application.Query.DomainEventHandlers;
using PetStore.Domain.Events;
using System.Reflection;

namespace Petstore.Swagger.Io.Api.Application.Config
{
    public class AutofacMediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
               .RegisterType<Mediator>()
               .As<IMediator>()
               .InstancePerLifetimeScope();

            //
            // REGISTER COMMAND CLASSES (they implement IRequestHandler) in assembly holding the Commands
            //
            builder.RegisterAssemblyTypes(typeof(CreatePetCommand).GetTypeInfo().Assembly)
               .AsClosedTypesOf(typeof(IRequestHandler<,>));

            //
            // REGISTER COMMAND HANDLERS (IRequestHandler)
            //
            builder.RegisterAssemblyTypes(typeof(CreatePetCommandHandler).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>));

            //
            // REGISTER DOMAIN EVENT HANDLERS (they implement INotificationHandler<>) in assembly holding the Domain Events 
            //
            builder.RegisterAssemblyTypes(typeof(CreatePetDomainEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            //
            // REGISTER VALIDATION HANDLERS
            //
            builder
                .RegisterAssemblyTypes(typeof(CreatePetValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            //
            // Misc
            //
            builder
                .RegisterAssemblyTypes(typeof(PetStoreEventDTO).GetTypeInfo().Assembly).AsImplementedInterfaces();
        }
    }
}