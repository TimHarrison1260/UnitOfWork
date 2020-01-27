using System;
using System.Collections.Generic;
using System.Data.Entity;
using Infrastructure.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Abstract class <c>Repository</c> is responsible for the basic CRUD operations
    /// of the Aggregate Model (TModel), stored in the DbContext (TEntity).
    /// </summary>
    /// <typeparam name="TModel">Aggregate Model being accessed by the repository</typeparam>
    /// <typeparam name="TEntity"><see cref="DbContext"/> containing the aggregate model</typeparam>
    public abstract class Repository<TModel, TEntity> : IRepository<TModel> where TModel: class, new() where TEntity: DbContext
    {
        private readonly DbContext _db;

        //  Parameterless ctor required to be able to use the class as a parameter to the create method in the factory for this repository
        //  It's not actually called, the create method calls the 2nd ctor, passing the context, to instantiate this class
        protected Repository(){}

        /// <summary>
        /// ctor: Accepts the instance of the <see cref="DbContext"/> as a parameter
        /// </summary>
        /// <param name="context">Instance of the <see cref="DbContext"/> </param>
        protected Repository(TEntity context)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the instance of the <see cref="DbContext"/> if required in the derived class
        /// </summary>
        internal TEntity Db => (TEntity) _db;

        /// <summary>
        /// Gets all instances of the Domain Aggregate class
        /// </summary>
        /// <returns>IEnumerable collection</returns>
        public virtual IEnumerable<TModel> GetAll()
        {
            IEnumerable<TModel> query = _db.Set<TModel>();
            return query;
        }

        /// <summary>
        /// Gets an instance of the domain aggregate class defined by
        /// the Id
        /// </summary>
        /// <param name="id">Id of the aggregate to be returned</param>
        /// <returns>Instance of the aggregate with the given id</returns>
        public virtual TModel GetById(int id)
        {
            var result = _db.Set<TModel>().Find(id);
            return result;
        }

        /// <summary>
        /// Creates an instance in the aggregate model passed in.
        /// </summary>
        /// <param name="model">Aggregate being created</param>
        /// <returns>An integer representing the Id of the aggregate just created</returns>
        public virtual int Create(TModel model)
        {
            throw new NotImplementedException();
            //_db.Set<TModel>().Add(model);

            //var modelState = _db.Entry(model).State;

            //var result = _db.SaveChanges();

            //return result;
        }

        /// <summary>
        /// Updates the instance of the aggregate model. 
        /// </summary>
        /// <param name="model">Aggregate being created</param>
        /// <returns>Returns True if the updates were successful, otherwise False</returns>
        public virtual bool Update(TModel model)
        {
            throw new NotImplementedException();
            //_db.Set<TModel>().Attach(model);
            //_db.Entry(model).State = EntityState.Modified;

            //var result = _db.SaveChanges();

            //var success = result > 0;

            //return success;
        }

        /// <summary>
        /// Deletes the instance of the aggregate model.
        /// </summary>
        /// <param name="model">Aggregate being created</param>
        /// <returns>Returns True if the delete was successful, otherwise False</returns>
        public bool Delete(TModel model)
        {
            throw new NotImplementedException();
        }

    }
}