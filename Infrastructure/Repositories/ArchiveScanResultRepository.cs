using System.Data.Entity;
using Core.Domain.Model;
using Infrastructure.Data;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Class <c>ArchiveScanResultRepository</c> is responsible for the CRUD operations
    /// for the <see cref="ArchiveScanResult"/> domain aggregate.  It uses the <see cref="SiteMonitorDbDataContext"/>
    /// </summary>
    /// <remarks>
    /// It is derived from the generic abstract <see cref="Repository{TModel,TEntity}"/> class
    /// </remarks>
    public class ArchiveScanResultRepository : Repository<ArchiveScanResult, SiteMonitorDbDataContext>
    {
        public ArchiveScanResultRepository()
        {}

        public ArchiveScanResultRepository(SiteMonitorDbDataContext context) : base(context)
        {}

        public ArchiveScanResultRepository(ISession<SiteMonitorDbDataContext> context) : base(context)
        {}

        public override int Create(ArchiveScanResult model)
        {
            //  Complex object, so override the base implementation completely
            //base.Create(model);
            Db.Set<ArchiveScanResult>().Add(model); // sets the state to Added

            var state = Db.Entry(model).State;

            //  Ensure the state for all test.results is set to added as well
            foreach (var archiveTestResult in model.TestResults)
            {
                Db.Entry(archiveTestResult).State = EntityState.Added;
            }

            //var result = Db.SaveChanges();
            return 0;
            //return result;
        }
    }
}