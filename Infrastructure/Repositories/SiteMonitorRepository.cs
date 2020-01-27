using Core.Domain.Model;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Class <c>SiteMonitorRepository</c> is responsible for the CRUD operations
    /// for the <see cref="WebSite"/> domain aggregate.  It uses the <see cref="SiteMonitorDbDataContext"/>
    /// </summary>
    /// <remarks>
    /// It is derived from the generic abstract <see cref="Repository{TModel,TEntity}"/> class
    /// </remarks>
    public class SiteMonitorRepository : Repository<WebSite, SiteMonitorDbDataContext>
    {
        public SiteMonitorRepository(){}

        public SiteMonitorRepository(SiteMonitorDbDataContext context) : base(context)
        {
        }
    }
}
