using Sketch7.Core.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Sketch7.Core.Entity.Repository
{
	public interface IRepository<TEntity> where TEntity : EntityBase
	{
		/// <summary>
		/// 	Add the specified entity.
		/// </summary>
		/// <param name="entity"> The entity. </param>
		void Add(TEntity entity);

		/// <summary>
		/// 	Add all specified entities.
		/// </summary>
		/// <param name="entities"> The entities. </param>
		void AddAll(params TEntity[] entities);

		/// <summary>
		/// 	Add all specified entities.
		/// </summary>
		/// <param name="entities"> The <see cref="IEnumerable{TEntity}" />. </param>
		void AddAll(IEnumerable<TEntity> entities);

		/// <summary>
		/// 	Updates changes of the existing entity. The caller must later call SaveChanges() on the repository explicitly to save the entity to database.
		/// </summary>
		/// <param name="entity"> The entity. </param>
		void Update(TEntity entity);

		/// <summary>
		/// 	Update all specified entities.
		/// </summary>
		/// <param name="entities"> The <see cref="IEnumerable{TEntity}" />. </param>
		void UpdateAll(params TEntity[] entities);

		/// <summary>
		/// 	Update all specified entities.
		/// </summary>
		/// <param name="entities"> The <see cref="IEnumerable{TEntity}" />. </param>
		void UpdateAll(IEnumerable<TEntity> entities);

		/// <summary>
		/// 	Deletes the specified entity.
		/// </summary>
		/// <param name="entity"> The entity. </param>
		void Delete(TEntity entity);

		/// <summary>
		/// 	Deletes one or many entities matching the specified criteria.
		/// </summary>
		/// <param name="predicate"> The where criteria. </param>
		void DeleteAll(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 	Delete all specified entities.
		/// </summary>
		/// <param name="entities"> The <see cref="IEnumerable{TEntity}" />. </param>
		void DeleteAll(params TEntity[] entities);

		/// <summary>
		/// 	Delete all specified entities.
		/// </summary>
		/// <param name="entities"> The <see cref="IEnumerable{TEntity}" />. </param>
		void DeleteAll(IEnumerable<TEntity> entities);

		/// <summary>
		/// 	Counts the specified entities.
		/// </summary>
		/// <returns> Returns total count. </returns>
		int Count();

		/// <summary>
		/// 	Counts entities with the specified criteria.
		/// </summary>
		/// <param name="predicate"> The where criteria. </param>
		/// <returns> Returns total count. </returns>
		int Count(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 	Determines whether any entities satisfy criteria.
		/// </summary>
		/// <param name="predicate"> The where criteria. </param>
		/// <returns> Returns total count. </returns>
		bool Any(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// 	Finds entity based on the entity primary keys as queryable.
		/// </summary>
		/// <param name="keyValues"> The values of the primary key for the entity to be found. </param>
		/// <returns> Returns <see cref="IQueryable{TEntity}" /> . </returns>
		IQueryable<TEntity> Get(params object[] keyValues);

		/// <summary>
		/// 	Get the query based on matching criteria.
		/// </summary>
		/// <param name="predicate"> The where criteria. </param>
		/// <returns> Returns <see cref="IQueryable{TEntity}" /> . </returns>
		IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);

		/// <summary>
		/// 	Finds entities not equal to the entity primary keys as queryable.
		/// </summary>
		/// <param name="keyValues"> The values of the primary key for the entity to not be found. </param>
		/// <returns> Returns <see cref="IQueryable{TEntity}" /> . </returns>
		IQueryable<TEntity> GetAllNotEqual(params object[] keyValues);

		/// <summary>
		/// 	Get the query predicting by the keySelector where in 'keyValues' .
		/// </summary>
		/// <typeparam name="TKey"> The key type. </typeparam>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="keyValues"> Collection to predicate. </param>
		/// <returns> Returns <see cref="IQueryable{TEntity}" /> . </returns>
		IQueryable<TEntity> GetWhereIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, params TKey[] keyValues);

		/// <summary>
		/// 	Get the query predicting by the keySelector where in 'keyValues' .
		/// </summary>
		/// <typeparam name="TKey"> The key type. </typeparam>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="keyValues"> Collection to predicate. </param>
		/// <returns> Returns <see cref="IQueryable{TEntity}" /> . </returns>
		IQueryable<TEntity> GetWhereIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, ICollection<TKey> keyValues);

		/// <summary>
		/// 	Get the query predicting by the keySelector where not in 'keyValues' .
		/// </summary>
		/// <typeparam name="TKey"> The key type. </typeparam>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="keyValues"> Collection to predicate. </param>
		/// <returns> Returns <see cref="IQueryable{TEntity}" /> . </returns>
		IQueryable<TEntity> GetWhereNotIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, params TKey[] keyValues);

		/// <summary>
		/// 	Get the query predicting by the keySelector where not in 'keyValues' .
		/// </summary>
		/// <typeparam name="TKey"> The key type. </typeparam>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="keyValues"> Collection to predicate. </param>
		/// <returns> Returns <see cref="IQueryable{TEntity}" /> . </returns>
		IQueryable<TEntity> GetWhereNotIn<TKey>(Expression<Func<TEntity, TKey>> keySelector, ICollection<TKey> keyValues);

		IEnumerable<EntityEntry<TEntity>> GetAllChanges();
	}
}