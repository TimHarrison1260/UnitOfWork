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
    /// <summary>
    /// Class <c>ServiceFactory</c> is responsible for instantiating
    /// the SiteMonitorService in the test application
    /// </summary>
    public class ServiceFactory
    {
        private readonly StandardKernel _kernel;

        /// <summary>
        /// ctor: accepts the IoC Kernel instance => Ninject
        /// </summary>
        /// <param name="kernel">Ninject Kernel instance</param>
        public ServiceFactory(StandardKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        /// <summary>
        /// Create instance of SiteMonitorService
        /// </summary>
        /// <returns>Instance of SiteMonitorService</returns>
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
        /// <returns>Instance of SiteMonitorService</returns>
        public ISiteMonitorService Create(bool useIoc)
        {
            return !useIoc ? Create() : _kernel.Get<SiteMonitorService>();
        }
    }
}