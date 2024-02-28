using Autofac;
using milescarrental.Application.Configuration.Emails;
using Module = Autofac.Module;

namespace milescarrental.Infrastructure.Emails
{
    public class EmailModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailSender>()
                .As<IEmailSender>()
                .InstancePerLifetimeScope();
        }
    }
}