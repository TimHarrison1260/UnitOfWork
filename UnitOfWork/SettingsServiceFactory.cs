using System;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Ninject;

namespace UnitOfWork
{
    public class SettingsServiceFactory
    {
        private readonly StandardKernel _kernel;

        /// <summary>
        /// ctor: accepts the IoC Kernel instance => Ninject
        /// </summary>
        /// <param name="kernel">Ninject Kernel instance</param>
        public SettingsServiceFactory(StandardKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        /// <summary>
        /// Create instance of SiteMonitorService
        /// </summary>
        /// <returns>Instance of SiteMonitorService</returns>
        public ISettingsService Create()
        {
            //  Infrastructure;
            // :EF DbContext
            var context = new SiteMonitorDbDataContext();
            //  Infrastructure: RepositoryFactory
            SiteMonitorSettingsRepository repository = new SiteMonitorSettingsRepository(context);

            //  Unit of Work
            ISettingsService settingsService = new SettingsService(context, new SiteMonitorRepositoryFactory(context));

            return settingsService; 
        }

        /// <summary>
        /// Create instance of SiteMonitorService from IoC
        /// </summary>
        /// <param name="useIoc">True create from Ioc, otherwise create</param>
        /// <returns>Instance of SiteMonitorService</returns>
        public ISettingsService Create(bool useIoc)
        {
            return !useIoc ? Create() : _kernel.Get<ISettingsService>();
        }
    }
        
}