using System;
using System.Data.Entity;
using Infrastructure.Interfaces.Factories;

namespace Infrastructure.Factories
{
    /// <summary>
    /// Abstract class <c>RepositoryFactory</c> responsible for creating the instance
    /// of the repository class given by the type of TRepository parameter.  It is intended for
    /// use with derived classes of the abstract class: <see cref="Infrastructure.Data.UnitOfWork{TEntity}"/>
    /// which implement the UnitOfWork pattern.  
    /// </summary>
    /// <remarks>
    /// This class is designed to avoid the need to inject multiple repository factory classes into the
    /// UnitOfWork class and reduce the need for configuration.  The repository classes must implement
    /// the same TEntity.
    /// </remarks>
    /// <typeparam name="TEntity">Instance of the <see cref="DbContext"/> used by</typeparam>
    /// <exception cref="ArgumentNullException">If the DbContext implementation is not provided</exception>
    /// <exception cref="ArgumentException">If the Type of repository does not exist</exception>
    public abstract class RepositoryFactory<TEntity> : IRepositoryFactory<TEntity> where TEntity: DbContext
    {
        private readonly DbContext _context;

        /// <summary>
        /// ctor: Ensures the correct <see cref="DbContext"/> is assigned to the Repository class when it is instantiated
        /// </summary>
        /// <param name="context">Instance of the <see cref="DbContext"/> used by the repository being instantiated</param>
        /// <exception cref="ArgumentNullException">If the DbContext implementation is not provided</exception>
        protected RepositoryFactory(TEntity context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates and instance of the repository of type TRepository
        /// </summary>
        /// <typeparam name="TRepository">Type of the Repository class being instantiated</typeparam>
        /// <returns>Instance of the type of repository class given by the TRepository parameter</returns>
        /// <exception cref="ArgumentException">If the Type of repository does not exist</exception>
        public virtual TRepository Create<TRepository>() where TRepository : class, new()
        {
            //  https://social.msdn.microsoft.com/forums/en-US/8ef58e3b-cb34-4d28-ba41-c683c64a031d/instantiating-a-generic-object-without-a-default-constructor
            //  Call the CreateInstance, non-generic, to control which constructor is used.  Here, the DbContext must be passed.
            var repositoryInstance = (TRepository) Activator.CreateInstance(typeof(TRepository), _context);

            return repositoryInstance ?? throw new ArgumentException($"Type of repository {typeof(TRepository)} is undefined");
        }
        
    }
}