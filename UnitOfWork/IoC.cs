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
            //  This binding is used when the ctor signature reference ctor(ISession<TEntity> context) => intended when using repositories directly.
            Bind<Infrastructure.Interfaces.Data.ISession<Infrastructure.Data.SiteMonitorDbDataContext>>()
                .To<Infrastructure.Data.SiteMonitorDbDataContext>()
                .InSingletonScope();    // Ensure same instance is always passed
            //  This binding is used when the ctor signature references ctor(TEntity context) => intended when using the UnitOrWork class to control transactions.
            Bind<Infrastructure.Data.SiteMonitorDbDataContext>()
                .ToSelf()
                .InSingletonScope();    // Ensure same instance is always passed

            //  Bindings for the RepositoryFactory, used within the UnitOfWork class, one needed for each DbContext (TEntity)
            //  This replaces the need for individual repositories to be configured.
            Bind<Infrastructure.Interfaces.Factories.IRepositoryFactory<Infrastructure.Data.SiteMonitorDbDataContext>>()
                .To<Infrastructure.Factories.SiteMonitorRepositoryFactory>();

            //  Binding to set up the individual services to the Core project interfaces.
            Bind<Core.Interfaces.Services.IMemberService>()
                .To<Infrastructure.Services.MemberService>();
            Bind<Core.Interfaces.Services.ISiteMonitorService>()
                .To<Infrastructure.Services.SiteMonitorService>();
            Bind<Core.Interfaces.Services.ISettingsService>()
                .To<Infrastructure.Services.SettingsService>();
            Bind<Core.Interfaces.Services.ISiteMonitorArchiveService>()
                .To<Core.Domain.Services.SiteMonitorArchiveService>();


            //  Binding for a specific repository, it can still be used within the UnitOfWork
            Bind<Infrastructure.Interfaces.Repositories.ISiteMonitorSettingsRepository>()
                .To<Infrastructure.Repositories.SiteMonitorSettingsRepository>();


            //  Configure the Mappers => Core project only => domain logic
            Bind<Core.Interfaces.Mappers.IMapper<Core.Domain.Model.ScanResult, Core.Domain.Model.ArchiveScanResult>>()
                .To<Core.Domain.Mappers.MapScanResultToArchiveScanResult>();
            Bind<Core.Interfaces.Mappers.IMapper<Core.Domain.Model.TestResult, Core.Domain.Model.ArchiveTestResult>>()
                .To<Core.Domain.Mappers.MapTestResultToArchiveTestResult>();

        }
    }
}