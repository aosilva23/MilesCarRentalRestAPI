using Autofac;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using milescarrental.Application;
using milescarrental.Application.Configuration.Data;
using milescarrental.Domain.SeedWork;
using milescarrental.Infrastructure.Domain;
using milescarrental.Infrastructure.SeedWork;

namespace milescarrental.Infrastructure.Database
{
    public class DataAccessModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;

        public DataAccessModule(string databaseConnectionString)
        {
            this._databaseConnectionString = databaseConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StronglyTypedIdValueConverterSelector>()
                .As<IValueConverterSelector>()
                .InstancePerLifetimeScope();
        }
    }
}