using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Interfaces.Services;
using Infrastructure.Services;
using Ninject;
using Ninject.Modules;

namespace UnitOfWork
{
    /// <summary>
    /// Application IoC module: <see cref="NinjectModule"/>
    /// </summary>
    public class IoC: NinjectModule
    {
        /// <summary>
        /// Configure the IoC container
        /// </summary>
        public override void Load()
        {
            Bind<ISession<SiteMonitorDbDataContext>>()
                .To<SiteMonitorDbDataContext>()
                .InSingletonScope();    // Ensure same instance is always passed

            Bind<IRepositoryFactory<SiteMonitorDbDataContext>>()
                .To<SiteMonitorRepositoryFactory>();

            Bind<ISiteMonitorService>()
                .To<SiteMonitorService>();

        }
    }
}