using System.Data.Entity;

namespace Infrastructure.Interfaces.Data
{
    /// <summary>
    /// Interface <code>ISession{TEntity}</code> is responsible for allowing an
    /// IOC framework to manage the lifecycle of a <see cref="DbContext"/>.  It
    /// should be implemented by the specific DbContext implementation, such
    /// as <see cref="Infrastructure.Data.SiteMonitorDbDataContext"/> for example.
    /// </summary>
    /// <remarks>
    /// Commonly used in MVC application, to manage the lifecycle as "per HttpRequest"
    /// to ensure that all references to the DbContext are serviced by the IOC with
    /// the same instance.
    /// </remarks>
    /// <typeparam name="TEntity">Represents the instance of the <see cref="DbContext"/> being managed</typeparam>
    public interface ISession<TEntity> where TEntity : DbContext
    {
        int SaveChanges();
    }
}