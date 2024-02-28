using Autofac;
//using milescarrental.Application.Customers.DomainServices;
using milescarrental.Domain.ForeignExchange;

namespace milescarrental.Infrastructure.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<ForeignExchange.ForeignExchange>()
                .As<IForeignExchange>()
                .InstancePerLifetimeScope();
        }
    }
}