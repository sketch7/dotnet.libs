using Sketch7.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	// Stefan Cruysberghs, July 2008, http://www.scip.be
	/// <summary>
	/// 	AsHierarchy extension methods for LINQ to Objects IEnumerable
	/// </summary>
	public static class LinqToObjectsExtensionMethods
	{
		private static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity, TKey>(
			IEnumerable<TEntity> allItems,
			TEntity parentItem,
			Func<TEntity, TKey> idProperty,
			Func<TEntity, TKey?> parentIdProperty,
			TKey? rootItemId,
			int maxDepth,
			int depth)
			where TEntity : class
			where TKey : struct
		{
			IEnumerable<TEntity> childs;

			if (rootItemId.HasValue)
			{
				childs = allItems.Where(i => idProperty(i).Equals(rootItemId));
			}
			else
			{
				if (parentItem == null)
				{
					childs = allItems.Where(i => parentIdProperty(i).Equals(default(TKey?)));
				}
				else
				{
					childs = allItems.Where(i => parentIdProperty(i).Equals(idProperty(parentItem)));
				}
			}

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
										CreateHierarchy(allItems.AsEnumerable(), item, idProperty, parentIdProperty, default(TKey?), maxDepth, depth),
								Depth = depth,
								Parent = parentItem
							};
				}
			}
		}

		/// <summary>
		/// 	LINQ to Objects (IEnumerable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <typeparam name="TKey"> Property of entity class </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="parentKeySelector"> A function to extract the parent key from an element. </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TKey>(
			this IEnumerable<TEntity> allItems,
			Func<TEntity, TKey> keySelector,
			Func<TEntity, TKey?> parentKeySelector)
			where TEntity : class
			where TKey : struct
			=> CreateHierarchy(allItems, default(TEntity), keySelector, parentKeySelector, default(TKey?), 0, 0);

		/// <summary>
		/// 	LINQ to Objects (IEnumerable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <typeparam name="TKey"> Property of entity class </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="parentKeySelector"> A function to extract the parent key from an element. </param>
		/// <param name="rootItemId"> Value of root item Id/Key </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TKey>(
			this IEnumerable<TEntity> allItems,
			Func<TEntity, TKey> keySelector,
			Func<TEntity, TKey?> parentKeySelector,
			TKey? rootItemId)
			where TEntity : class
			where TKey : struct
			=> CreateHierarchy(allItems, default(TEntity), keySelector, parentKeySelector, rootItemId, 0, 0);

		/// <summary>
		/// 	LINQ to Objects (IEnumerable) AsHierachy() extension method
		/// </summary>
		/// <typeparam name="TEntity"> Entity class </typeparam>
		/// <typeparam name="TKey"> Property of entity class </typeparam>
		/// <param name="allItems"> Flat collection of entities </param>
		/// <param name="keySelector"> A function to extract the key from an element. </param>
		/// <param name="parentKeySelector"> A function to extract the parent key from an element. </param>
		/// <param name="rootItemId"> Value of root item Id/Key </param>
		/// <param name="maxDepth"> Maximum depth of tree </param>
		/// <returns> Hierarchical structure of entities </returns>
		public static IEnumerable<HierarchyNode<TEntity>> AsHierarchy<TEntity, TKey>(
			this IEnumerable<TEntity> allItems,
			Func<TEntity, TKey> keySelector,
			Func<TEntity, TKey?> parentKeySelector,
			TKey? rootItemId,
			int maxDepth)
			where TEntity : class
			where TKey : struct
			=> CreateHierarchy(allItems, default(TEntity), keySelector, parentKeySelector, rootItemId, maxDepth, 0);

		public static IEnumerable<T> ToFlattenHierarchy<T>(this IEnumerable<HierarchyNode<T>> source) where T : class
		{
			if (source == null) throw new ArgumentNullException(nameof(source), "Argument must be set.");

			var result = new List<T>().ToList();

			void Traverse(IEnumerable<HierarchyNode<T>> x)
			{
				foreach (var node in x)
				{
					result.Add(node.Entity);
					if (node.ChildNodes != null)
						Traverse(node.ChildNodes);
				}
			}

			Traverse(source);

			return result;
		}
	}
}