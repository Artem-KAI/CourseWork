using Autofac;

using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using DAL.Interfaces;

namespace WebAPI.Infrastructure
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataStore>()
                .As<IDataStore>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CredentialManager>()
                .As<ICredentialManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ScheduleManager>()
                .As<IScheduleManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<StatisticsManager>()
                .As<IStatisticsManager>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserManager>()
                .As<IUserManager>()
                .InstancePerLifetimeScope();
        }
    }
}
