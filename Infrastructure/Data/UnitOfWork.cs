using System;
using System.Data.Entity;
using System.Linq;
using Infrastructure.Interfaces.Factories;

namespace Infrastructure.Data
{
    /// <summary>
    /// Abstract class <c>UnitOfWork</c> is an implementation of the Unit Of Work pattern.  It is intended
    /// primarily for use with Entity Framework transactions to control updates to multiple
    /// entities making use of multiple repositories to access the entities.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is an abstract generic class, intended to allow the derived class to reference the necessary
    /// concrete DbContext and Repository classes.  It is designed to work wth or without the use of EF transactions.  
    /// </para>
    /// <list type="bullet">
    /// <listheader>References</listheader>
    /// <item>
    /// <description>Fowler, Martin, 2003, Patterns of Enterprise Application Architecture, pp184 </description>
    /// </item>
    /// <item>
    /// <description>Danylko, J., 2015, A Better Entity Framework Unit Of Work Pattern [online], Available at https://www.danylkoweb.com/Blog/a-better-entity-framework-unit-of-work-pattern-DD</description>
    /// </item>
    /// <item>
    /// <description>Greer, D., 2015, Survey of Entity Framework Unit of Work Patterns [online], Available at https://lostechies.com/derekgreer/2015/11/01/survey-of-entity-framework-unit-of-work-patterns/</description>
    /// </item>
    /// <item>
    /// <description>Dykstra, T., 2013, Implementing the Repository and Unit of Work Patterns in an ASP.NET MVC Application [online], Available at https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <typeparam name="TEntity">Represents the instance of the <see cref="DbContext"/> being used</typeparam>
    public abstract class UnitOfWork<TEntity> where TEntity: DbContext, IDisposable
    {

        private readonly DbContext _context;
        private readonly IRepositoryFactory<TEntity> _repositoryFactory;
        private DbContextTransaction _dbContextTransaction;

        /// <summary>
        /// ctor: protected constructor for the <see cref="UnitOfWork{TEntity}"/> class.  
        /// </summary>
        /// <param name="context">Instance of the <see cref="DbContext"/> class (Entity Framework), represented by the TEntity</param>
        /// <param name="repositoryFactory">Instance of the abstract <see cref="Infrastructure.Factories.RepositoryFactory"/></param>
        protected UnitOfWork(TEntity context, IRepositoryFactory<TEntity> repositoryFactory)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            _dbContextTransaction = null;
        }

        /// <summary>
        /// Generic method, to get the instance of the specific repository class for the Domain Aggregate, following
        /// the principle of having a repository specific to each aggregate.
        /// </summary>
        /// <typeparam name="TModel">Type of Domain Aggregate class</typeparam>
        /// <returns>Instance of the appropriate repository class for the specified aggregate class</returns>
        public TModel GetRepository<TModel>() where TModel: class, new()
        {
            var repository = _repositoryFactory.Create<TModel>();
            return repository;
        }

        /// <summary>
        /// Exposes the <c>Database.BeginTransaction()"</c> method on the <see cref="DbContext"/> class.  If the transaction
        /// has already been started, it returns the existing transaction, otherwise it creates a new transaction.  This was
        /// introduce in EF version 6.
        /// </summary>
        /// <returns>Instance of the Database Transaction</returns>
        public DbContextTransaction BeginTransaction()
        {
            //  Begin a new transaction if one doesn't already exist
            return _dbContextTransaction ?? (_dbContextTransaction = _context.Database.BeginTransaction());
        }

        /// <summary>
        /// Applies the changes to the entities within the <see cref="DbContext"/>.  If a transaction has been started as is
        /// currently in progress, the SaveChanges() method recognises this and works within the transaction.  If no such
        /// transaction is in progress, it behaves as normal by applying the changes to the underlying database.  This is
        /// normal EF behaviour, and uses EF's internal Unit of Work and transaction processing.
        /// </summary>
        /// <returns>The number of changes applied to all entities within the aggregate</returns>
        public int SaveChanges()
        {
            var changesApplied = _context.SaveChanges();
            return changesApplied;
        }

        /// <summary>
        /// Applies the changes to all Entities within the aggregate to the underlying database.  If a transaction
        /// is currently open, it calls a transaction.commit(), if not, then it calls a context.SaveChanges().
        /// </summary>
        public void Commit()
        {
            if (_dbContextTransaction != null)
                _dbContextTransaction.Commit();
            else
                _context.SaveChanges();
        }

        /// <summary>
        /// Performs a transaction.RollBack() to cancel / undo the changes to all entities in the aggregate.  If
        /// no transaction is open, it reloads the tracked entities with the original values.  It will not work
        /// if a SaveChanges() has been performed in the interim, when
        /// </summary>
        public void RollBack()
        {
            if (_dbContextTransaction != null)
                _dbContextTransaction.Rollback();
            else
                _context.ChangeTracker
                    .Entries()
                    .ToList()
                    .ForEach(x => x.Reload());
        }


        //  IDisposable 
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
