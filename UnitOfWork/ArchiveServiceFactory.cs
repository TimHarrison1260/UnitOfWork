using System;
using Core.Domain.Mappers;
using Core.Domain.Model;
using Core.Domain.Services;
using Core.Interfaces.Mappers;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Services;
using Ninject;
using Ninject.Modules;

namespace UnitOfWork
{
    /// <summary>
    /// Class <c>ArchiveServiceFactory</c> is responsible for instantiating
    /// the SiteMonitorArchiveService in the test application
    /// </summary>
    public class ArchiveServiceFactory
    {
        private readonly StandardKernel _kernel;

        /// <summary>
        /// ctor: accepts the IoC Kernel instance => Ninject
        /// </summary>
        /// <param name="kernel">Ninject Kernel instance</param>
        public ArchiveServiceFactory(StandardKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        /// <summary>
        /// Create instance of SiteMonitorArchiveService
        /// </summary>
        /// <returns>Instance of SiteMonitorArchiveService</returns>
        public ISiteMonitorArchiveService Create()
        {
            //  Infrastructure;
            // :EF DbContext
            var context = new SiteMonitorDbDataContext();
            //  Infrastructure: RepositoryFactory
            IRepositoryFactory<SiteMonitorDbDataContext> repositoryFactory = new SiteMonitorRepositoryFactory(context);

            //  Unit of Work
            ISiteMonitorService siteMonitorService = new SiteMonitorService(context, repositoryFactory);

            //  Domain: Mappers
            IMapper<TestResult, ArchiveTestResult> testResultMapper = new MapTestResultToArchiveTestResult();
            IMapper<ScanResult, ArchiveScanResult> scanResultMapper = new MapScanResultToArchiveScanResult(testResultMapper);
            //  Domain: Archive service
            ISiteMonitorArchiveService siteMonitorArchiveService = new SiteMonitorArchiveService(siteMonitorService, scanResultMapper);

            return siteMonitorArchiveService; 
        }

        /// <summary>
        /// Create instance of SiteMonitorArchiveService from IoC
        /// </summary>
        /// <param name="useIoc">True create from Ioc, otherwise create</param>
        /// <returns>Instance of SiteMonitorArchiveService</returns>
        public ISiteMonitorArchiveService Create(bool useIoc)
        {
            return !useIoc ? Create() : _kernel.Get<ISiteMonitorArchiveService>();
        }
    }
}