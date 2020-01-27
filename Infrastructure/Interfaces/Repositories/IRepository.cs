using System.Collections.Generic;

namespace Infrastructure.Interfaces.Repositories
{
    /// <summary>
    /// Interface <c>IRepository</c> defines the interface for the abstract
    /// <see cref="Infrastructure.Repositories.Repository{TModel, TEntity}"/>
    /// class.  
    /// </summary>
    /// <remarks>
    /// It can be used to configure the Repository implementations through
    /// an IOC or can be inherited allowing further specific methods to be
    /// included within the repository
    /// </remarks>
    /// <typeparam name="TModel">The Domain Model, Aggregate, class the repository is controlling access for.</typeparam>
    public interface IRepository<TModel> where TModel: class, new()
    {
        /// <summary>
        /// Gets all instances of the Domain Aggregate class
        /// </summary>
        /// <returns>IEnumerable collection</returns>
        IEnumerable<TModel> GetAll();

        /// <summary>
        /// Gets an instance of the domain aggregate class defined by
        /// the Id
        /// </summary>
        /// <param name="id">Id of the aggregate to be returned</param>
        /// <returns>Instance of the aggregate with the given id</returns>
        TModel GetById(int id);
        
        /// <summary>
        /// Creates an instance in the aggregate model passed in.
        /// </summary>
        /// <param name="model">Aggregate being created</param>
        /// <returns>An integer representing the Id of the aggregate just created</returns>
        int Create(TModel model);

        /// <summary>
        /// Updates the instance of the aggregate model. 
        /// </summary>
        /// <param name="model">Aggregate being created</param>
        /// <returns>Returns True if the updates were successful, otherwise False</returns>
        bool Update(TModel model);

        /// <summary>
        /// Deletes the instance of the aggregate model.
        /// </summary>
        /// <param name="model">Aggregate being created</param>
        /// <returns>Returns True if the delete was successful, otherwise False</returns>
        bool Delete(TModel model);
        
    }
}