using Infrastructure.Data;

namespace Infrastructure.Factories
{
    /// <summary>
    /// Class <c>SiteMonitorRepositoryFactory</c> is the implementation of the
    /// abstract <see cref="RepositoryFactory{TEntity}"/> class.
    /// It uses the <see cref="SiteMonitorDbDataContext"/> class as TEntity.
    /// </summary>
    public class SiteMonitorRepositoryFactory : RepositoryFactory<SiteMonitorDbDataContext>
    {
        public SiteMonitorRepositoryFactory(SiteMonitorDbDataContext context) : base(context)
        {
        }
    }
}
