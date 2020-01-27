using System.Data.Entity;

namespace Infrastructure.Interfaces.Factories
{
    /// <summary>
    /// Interface <c>IRepositoryFactory{TEntity}</c> describes the interface for
    /// the abstract <see cref="Infrastructure.Factories.RepositoryFactory{TEntity}"/> class.
    /// It is responsible for creating instances of repositories, defined by TRepository,
    /// using the given TEntity.
    /// </summary>
    /// <remarks>
    /// This class is designed to avoid the need to inject multiple repository factory classes into the
    /// UnitOfWork class and reduce the need for configuration.  The repository classes must implement
    /// the same TEntity.
    /// </remarks>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryFactory<TEntity> where TEntity : DbContext
    {
        /// <summary>
        /// Creates and instance of the repository of type TRepository
        /// </summary>
        /// <typeparam name="TRepository">Type of the Repository class being instantiated</typeparam>
        /// <returns>Instance of the type of repository class given by the TRepository parameter</returns>
        TRepository Create<TRepository>() where TRepository: class, new();
    }
}