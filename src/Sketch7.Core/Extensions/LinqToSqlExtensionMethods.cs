using Sketch7.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	// Stefan Cruysberghs, July 2008, http://www.scip.be
	/// <summary>
	/// 	AsHierarchy extension methods for LINQ to SQL IQueryable
	/// </summary>
	public static class LinqToSqlExtensionMethods
	{
		#region ChikoHierarchy

		private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity, TKey>(
			IQueryable<TEntity> allItems,
			TEntity parentItem,
			Expression<Func<TEntity, TKey>>
				keySelector,
			Expression<Func<TEntity, TKey?>>
				parentKeySelector,
			TKey? rootItemId,
			int maxDepth,
			int depth
		)
			where TEntity : class
			where TKey : struct
		{
			var parameter = Expression.Parameter(typeof(TEntity), "e");
			Expression<Func<TEntity, bool>> predicate;

			if (rootItemId.HasValue)
			{
				var parentIdPropInfo = (PropertyInfo)((MemberExpression)parentKeySelector.Body).Member;
				Expression left = Expression.Property(parameter, parentIdPropInfo);
				left = Expression.Convert(left, keySelector.ReturnType);

				Expression right = Expression.Constant(rootItemId, keySelector.ReturnType);
				predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
			}
			else
			{
				var parentIdPropInfo = (PropertyInfo)((MemberExpression)parentKeySelector.Body).Member;
				Expression left = Expression.Property(parameter, parentIdPropInfo);

				if (parentItem == null)
				{
					predicate =
						Expression.Lambda<Func<TEntity, bool>>(
							Expression.Equal(left, Expression.Constant(null)), parameter);
				}
				else
				{
					Expression body = keySelector;
					PropertyInfo propInfo;
					if (body is LambdaExpression)
						body = ((LambdaExpression)body).Body;
					switch (body.NodeType)
					{
						case ExpressionType.MemberAccess:
							propInfo = (PropertyInfo)((MemberExpression)body).Member;
							break;
						case ExpressionType.Convert:
							var ue = body as UnaryExpression;
							var me = ue?.Operand as MemberExpression;
							propInfo = (PropertyInfo)me.Member;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					//var idPropInfo = (PropertyInfo)((MemberExpression)idProperty.Body).Member;

					left = Expression.Convert(left, keySelector.ReturnType);
					Expression right = Expression.Constant(propInfo.GetValue(parentItem, null), typeof(TKey));

					predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
				}
			}

			IEnumerable<TEntity> childs = allItems.Where(predicate).ToList();

			if (childs.Any())
			{
				depth++;

				if ((depth <= maxDepth) || (maxDepth == 0))
				{
					foreach (var item in childs)
						yield return
							new HierarchyNode<TEntity>
							{
								Entity = item,
								ChildNodes =
										CreateHierarchy(allItems, item, keySelector, parentKeySelector, default(TKey?), maxDepth, depth),
								Depth = depth,
								Parent = parentItem
							};
				}
			}
		}

		/// <summary>
		/// 	LINQ to SQL (IQueryable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <typeparam name="TKey"> Property of entity class. </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="parentKeySelector"> A function to extract the parent key from an element. </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TKey>(
			this IQueryable<TEntity> allItems,
			Expression<Func<TEntity, TKey>> keySelector,
			Expression<Func<TEntity, TKey?>> parentKeySelector)
			where TEntity : class
			where TKey : struct => CreateHierarchy(allItems, null, keySelector, parentKeySelector, default(TKey?), 0, 0);

		/// <summary>
		/// 	LINQ to SQL (IQueryable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <typeparam name="TKey"> Property of entity class. </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="parentKeySelector"> A function to extract the parent key from an element. </param>
		/// <param name="rootItemId"> Value of root item Id/Key </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TKey>(
			this IQueryable<TEntity> allItems,
			Expression<Func<TEntity, TKey>> keySelector,
			Expression<Func<TEntity, TKey?>> parentKeySelector,
			//string parentIdProperty,
			TKey? rootItemId)
			where TEntity : class
			where TKey : struct => CreateHierarchy(allItems, null, keySelector, parentKeySelector, rootItemId, 0, 0);

		/// <summary>
		/// 	LINQ to SQL (IQueryable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <typeparam name="TKey"> Property of entity class. </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="parentKeySelector"> A function to extract the parent key from an element. </param>
		/// <param name="rootItemId"> Value of root item Id/Key </param>
		/// <param name="maxDepth"> Maximum depth of tree </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TKey>(
			this IQueryable<TEntity> allItems,
			Expression<Func<TEntity, TKey>> keySelector,
			Expression<Func<TEntity, TKey?>> parentKeySelector,
			//string parentIdProperty,
			TKey? rootItemId,
			int maxDepth)
			where TEntity : class
			where TKey : struct => CreateHierarchy(allItems, null, keySelector, parentKeySelector, rootItemId, maxDepth, 0);

		#endregion

		private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity>(IQueryable<TEntity> allItems,
																					TEntity parentItem,
																					string propertyNameId,
																					string propertyNameParentId,
																					object rootItemId,
																					int maxDepth,
																					int depth) where TEntity : class
		{
			var parameter = Expression.Parameter(typeof(TEntity), "e");
			Expression<Func<TEntity, bool>> predicate;

			if (rootItemId != null)
			{
				Expression left = Expression.Property(parameter, propertyNameId);
				left = Expression.Convert(left, rootItemId.GetType());
				Expression right = Expression.Constant(rootItemId);

				predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
			}
			else
			{
				if (parentItem == null)
				{
					predicate =
						Expression.Lambda<Func<TEntity, bool>>(
							Expression.Equal(Expression.Property(parameter, propertyNameParentId),
											 Expression.Constant(null)), parameter);
				}
				else
				{
					Expression left = Expression.Property(parameter, propertyNameParentId);
					left = Expression.Convert(left, parentItem.GetType().GetProperty(propertyNameId).PropertyType);
					Expression right = Expression.Constant(parentItem.GetType().GetProperty(propertyNameId).GetValue(parentItem, null));

					predicate = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), parameter);
				}
			}

			IEnumerable<TEntity> childs = allItems.Where(predicate).ToList();

			if (childs.Any())
			{
				depth++;

				if (depth <= maxDepth || maxDepth == 0)
				{
					foreach (var item in childs)
						yield return
							new HierarchyNode<TEntity>
							{
								Entity = item,
								ChildNodes =
										CreateHierarchy(allItems, item, propertyNameId, propertyNameParentId, null, maxDepth, depth),
								Depth = depth,
								Parent = parentItem
							};
				}
			}
		}

		/// <summary>
		/// 	LINQ to SQL (IQueryable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="propertyNameId"> String with property name of Id/Key </param>
		/// <param name="propertyNameParentId"> String with property name of parent Id/Key </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
			this IQueryable<TEntity> allItems,
			string propertyNameId,
			string propertyNameParentId) where TEntity : class => CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, null, 0, 0);

		/// <summary>
		/// 	LINQ to SQL (IQueryable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="propertyNameId"> String with property name of Id/Key </param>
		/// <param name="propertyNameParentId"> String with property name of parent Id/Key </param>
		/// <param name="rootItemId"> Value of root item Id/Key </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
			this IQueryable<TEntity> allItems,
			string propertyNameId,
			string propertyNameParentId,
			object rootItemId) where TEntity : class => CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, rootItemId, 0, 0);

		/// <summary>
		/// 	LINQ to SQL (IQueryable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="propertyNameId"> String with property name of Id/Key </param>
		/// <param name="propertyNameParentId"> String with property name of parent Id/Key </param>
		/// <param name="rootItemId"> Value of root item Id/Key </param>
		/// <param name="maxDepth"> Maximum depth of tree </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity>(
			this IQueryable<TEntity> allItems,
			string propertyNameId,
			string propertyNameParentId,
			object rootItemId,
			int maxDepth) where TEntity : class => CreateHierarchy(allItems, null, propertyNameId, propertyNameParentId, rootItemId, maxDepth, 0);
	}
}