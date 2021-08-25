using Autofac;
using MediatR;

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
        }
    }
}