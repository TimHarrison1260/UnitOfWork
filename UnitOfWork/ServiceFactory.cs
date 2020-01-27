using System;
using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Interfaces.Services;
using Infrastructure.Services;
using Ninject;
using Ninject.Modules;

namespace UnitOfWork
{
    public class ServiceFactory
    {
        private readonly StandardKernel _kernel;

        public ServiceFactory(StandardKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        /// <summary>
        /// Create instance of SiteMonitorService
        /// </summary>
        /// <returns></returns>
        public ISiteMonitorService Create()
        {
            var context = new SiteMonitorDbDataContext();
            var repositoryFactory = new SiteMonitorRepositoryFactory(context);

            //  Unit of Work pattern
            return new SiteMonitorService(context, repositoryFactory); 
        }

        /// <summary>
        /// Create instance of SiteMonitorService from IoC
        /// </summary>
        /// <param name="useIoc">True create from Ioc, otherwise create</param>
        /// <returns></returns>
        public ISiteMonitorService Create(bool useIoc)
        {
            return !useIoc ? Create() : _kernel.Get<SiteMonitorService>();
        }
    }
}