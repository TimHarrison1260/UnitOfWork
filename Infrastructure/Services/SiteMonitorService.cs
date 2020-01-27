using System;
using System.Linq;
using Infrastructure.Data;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Interfaces.Services;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    /// <summary>
    /// Class <c>SiteMonitorService</c> provides the access to the underlying <see cref="SiteMonitorDbDataContext"/>.
    /// It derives from the <see cref="UnitOfWork{TEntity}"/>, where TEntity is the <see cref="SiteMonitorDbDataContext"/>
    /// and follows the UnitOfWork design pattern to allow control over transactional updates.
    /// </summary>
    /// <remarks>
    /// Private instances of ALL required repositories should be defined.  They MUST all use the same
    /// instance of <see cref="SiteMonitorDbDataContext"/>.
    /// </remarks>
    public class SiteMonitorService : UnitOfWork<SiteMonitorDbDataContext>, ISiteMonitorService
    {
        //  private instances of ALL required repositories
        private SiteMonitorRepository _repository;

        public SiteMonitorService(SiteMonitorDbDataContext context, IRepositoryFactory<SiteMonitorDbDataContext> repositoryFactory) 
            : base(context, repositoryFactory)
        {
            _repository = base.GetRepository<SiteMonitorRepository>();
        }

        public bool Archive(DateTime archiveDate)
        {
            var objects = _repository.GetAll().ToList();
            var result = objects.Any();
            return result;
        }
    }
}