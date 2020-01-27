using System.Data.Entity;
using Core.Domain.Model;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Data
{
    /// <summary>
    /// Class <code>SiteMonitorDbDataContext</code> is the Entity Framework 6 class responsible
    /// for controlling access to the underlying database.  This is a standard implementation
    /// for the Entity Framework DbContext.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It implements the <see cref="DbContext"/> object (EF).
    /// </para>
    /// <para>
    /// It implements the <see cref="ISession{TEntity}"/>, to facilitate the use of
    /// IOC frameworks, which can be used to control the appropriate lifecycle of
    /// the DbContext.  The TEntity generic should reference this DbContext.
    /// </para>
    /// </remarks>
    public class SiteMonitorDbDataContext : DbContext, ISession<SiteMonitorDbDataContext>
    {
        /// <inheritdoc />
        /// <summary>
        /// ctor: ensure correct connection string is used.
        /// </summary>
        public SiteMonitorDbDataContext() : base("UoWTests")
        {
            //Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;

        }

        public DbSet<WebSite> WebSites { get; set; }

    }
}
