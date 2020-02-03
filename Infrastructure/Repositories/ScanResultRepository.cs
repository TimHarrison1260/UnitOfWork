using System.Data.Entity;
using Core.Domain.Model;
using Infrastructure.Data;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Class <c>ScanResultRepository</c> is responsible for the CRUD operations
    /// for the <see cref="ScanResult"/> domain aggregate.  It uses the <see cref="SiteMonitorDbDataContext"/>
    /// </summary>
    /// <remarks>
    /// It is derived from the generic abstract <see cref="Repository{TModel,TEntity}"/> class
    /// </remarks>
    public class ScanResultRepository : Repository<ScanResult, SiteMonitorDbDataContext>
    {
        public ScanResultRepository(){}

        public ScanResultRepository(SiteMonitorDbDataContext context) : base(context)
        {
        }

        public ScanResultRepository(ISession<SiteMonitorDbDataContext> context) : base(context)
        {
        }

        public override int Create(ScanResult model)
        {
            base.Create(model);

            foreach (var testResult in model.TestResults)
            {
                Db.Entry(testResult).State = EntityState.Added;
            }

            return 0;
        }

        public override bool Delete(ScanResult model)
        {
            base.Delete(model);

            foreach (var testResult in model.TestResults)
            {
                Db.Entry(testResult).State = EntityState.Deleted;
            }

            return true;
        }
    }
}