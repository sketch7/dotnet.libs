using Microsoft.EntityFrameworkCore;
using Sketch7.Core.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Sketch7.Core.Entity.Repository
{
	public abstract class Repository<TEntity, TContext> : IRepository<TEntity>
		where TEntity : EntityBase
		where TContext : DbContext
	{
		private readonly IEnumerable<PropertyInfo> _keyProperties;
		private TContext _dataContext;
		private readonly DbSet<TEntity> _dbset;

		protected Repository(IDatabaseFactory<TContext> databaseFactory)
		{
			DatabaseFactory = databaseFactory;
			_dbset = DataContext.Set<TEntity>();
			_keyProperties = Activator.CreateInstance<TEntity>().KeyProperties;
		}

		private IDatabaseFactory<TContext> DatabaseFactory { get; set; }

		private TContext DataContext => _dataContext ?? (_dataContext = DatabaseFactory.Get());

		private IQueryable<TEntity> WhereComposite(Expression<Func<TEntity, bool>> predicate, params object[] keyValues)
			=> GetAll(predicate).WhereComposite(_keyProperties, keyValues);

		#region Create/Update/Delete

		public void Add(TEntity entity)
		{
			if (entity is IModifiedEntity modifiedEntity)
				modifiedEntity.CreatedDate = DateTime.UtcNow;
			_dbset.Add(entity);
		}

		private void Add(IEnumerable<TEntity> entities)
		{
			//http://stackoverflow.com/questions/5943394/why-is-inserting-entities-in-ef-4-1-so-slow-compared-to-objectcontext
			_dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
			foreach (var entity in entities)
				Add(entity);
			_dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
		}

		public void AddAll(params TEntity[] entities) => Add(entities);

		public void AddAll(IEnumerable<TEntity> entities) => Add(entities);

		public void Update(TEntity entity)
		{
			if (entity is IModifiedEntity)
				(entity as IModifiedEntity).LastModified = DateTime.UtcNow;

			_dbset.Attach(entity);
			_dataContext.Entry(entity).State = EntityState.Modified;
		}

		private void Update(IEnumerable<TEntity> entities)
		{
			_dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
			foreach (var entity in entities)
				Update(entity);
			_dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
		}

		public void UpdateAll(params TEntity[] entities) => Update(entities);

		public void UpdateAll(IEnumerable<TEntity> entities) => Update(entities);

		public void Delete(TEntity entity)
		{
			var dbEntityEntry = _dataContext.Entry(entity);
			if (dbEntityEntry.State != Microsoft.EntityFrameworkCore.EntityState.Deleted)
			{
				dbEntityEntry.State = EntityState.Deleted;
			}
			else
			{
				_dbset.Attach(entity);
				_dbset.Remove(entity);
			}
		}

		public void DeleteAll(Expression<Func<TEntity, bool>> predicate)
		{
			var entities = _dbset.Where(predicate).AsEnumerable();
			Delete(entities);
		}

		private void Delete(IEnumerable<TEntity> entities)
		{
			_dataContext.ChangeTracker.AutoDetectChangesEnabled = false;
			foreach (var entity in entities)
				_dbset.Remove(entity);
			_dataContext.ChangeTracker.AutoDetectChangesEnabled = true;
		}

		public void DeleteAll(params TEntity[] entities) => Delete(entities);

		public void DeleteAll(IEnumerable<TEntity> entities) => Delete(entities);

		#endregion

		public int Count() => _dbset.Count();

		public int Count(Expression<Func<TEntity, bool>> predicate) => _dbset.Where(predicate).Count();

		public bool Any(Expression<Func<TEntity, bool>> predicate) => _dbset.Any(predicate);

		public IQueryable<TEntity> Get(params object[] keyValues) => WhereComposite(null, keyValues);

		public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
		{
			var query = _dbset.AsQueryable();

			if (predicate != null)
				query = query.Where(predicate);

			return query;
		}

		public IQueryable<TEntity> GetAllNotEqual(params object[] keyValues)
			=> GetAll().WhereCompositeNotEqual(_keyProperties, keyValues);

		public IQueryable<TEntity> GetWhereIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, params TKey[] keyValues)
			=> GetAll().WhereIn(keySelector, keyValues);

		public IQueryable<TEntity> GetWhereIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, ICollection<TKey> keyValues)
			=> GetAll().WhereIn(keySelector, keyValues);

		public IQueryable<TEntity> GetWhereNotIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, params TKey[] keyValues)
			=> GetAll().WhereNotIn(keySelector, keyValues);

		public IQueryable<TEntity> GetWhereNotIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, ICollection<TKey> keyValues)
			=> GetAll().WhereNotIn(keySelector, keyValues);

		public IEnumerable<EntityEntry<TEntity>> GetAllChanges()
			=> DataContext.ChangeTracker.Entries<TEntity>().Where(x => x.State != EntityState.Unchanged);
	}
}