using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Interfaces.Data;
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

        private readonly bool _callSaveChanges;

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
            _callSaveChanges = false;
        }

        /// <summary>
        /// ctor:
        /// </summary>
        /// <param name="context"></param>
        protected Repository(ISession<TEntity> context)
        {
            _db = (TEntity)context ?? throw new ArgumentNullException(nameof(context));
            _callSaveChanges = true;
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
        /// Gets all instances of the Domain Aggregate class
        /// that match the various criteria passed.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns>IEnumerable collection</returns>
        /// <remarks>
        /// See https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        /// for description of using this kind of method to ensure the sorting etc is done in the underlying database and not in memory.
        /// </remarks>
        public virtual IEnumerable<TModel> Get(
            Expression<Func<TModel, bool>> filter = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            string includeProperties = "")
        {
            //  Set up the query
            IQueryable<TModel> query = _db.Set<TModel>();

            //  Apply any filters to the query
            if (filter != null)
                query = query.Where(filter);

            //  Include any properties specified
            foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var results = orderBy != null ? orderBy(query).ToList() : query.ToList();
            return results;
        }

        /// <summary>
        /// Gets an instance of the domain aggregate class defined by
        /// the Id
        /// </summary>
        /// <param name="id">Id of the aggregate to be returned</param>
        /// <returns>Instance of the aggregate with the given id</returns>
        public virtual TModel Get(int id)
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
            _db.Set<TModel>().Add(model);

            if (!_callSaveChanges) return 0; // No changes have been persisted

            var result = _db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Updates the instance of the aggregate model. 
        /// </summary>
        /// <param name="model">Aggregate being created</param>
        /// <returns>Returns True if the updates were successful, otherwise False</returns>
        public virtual bool Update(TModel model)
        {
            _db.Set<TModel>().Attach(model);
            _db.Entry(model).State = EntityState.Modified;

            if (!_callSaveChanges) return true; // No changes have been persisted

            var result = _db.SaveChanges();
            var success = result > 0;
            return success;
        }

        /// <summary>
        /// Deletes the instance of the aggregate model.
        /// </summary>
        /// <param name="model">Aggregate being deleted</param>
        /// <returns>Returns True if the delete was successful, otherwise False</returns>
        /// <remarks>
        /// For info on the DbContext class see:
        /// https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbcontext?view=entity-framework-6.2.0
        /// 
        /// For combining method signatures, info about the DBSet{TEntity} class and the different
        /// methods, see:
        /// https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.dbset-1?view=entity-framework-6.2.0
        /// </remarks>
        public virtual bool Delete(TModel model)
        {
            if (_db.Entry(model).State == EntityState.Detached)
                _db.Set<TModel>().Attach(model);
            _db.Set<TModel>().Remove(model);

            if (!_callSaveChanges) return true; // No changes have been persisted yet

            var result = _db.SaveChanges();
            var success = result > 0;
            return success;
        }

        /// <summary>
        /// Deletes the instance of the aggregate model.
        /// </summary>
        /// <param name="id">Id of the Aggregate being deleted</param>
        /// <returns>Returns True if the delete was successful, otherwise False</returns>
        public virtual bool Delete(int id)
        {
            //  Get the instance of the TModel with the specified Id
            TModel model = _db.Set<TModel>().Find(id);
            if (model == null) return false;    //  Element with the Id was not found
            _db.Set<TModel>().Remove(model);

            if (!_callSaveChanges) return true; // No changes have been persisted yet

            var result = _db.SaveChanges();
            var success = result > 0;
            return success;
        }
    }
}