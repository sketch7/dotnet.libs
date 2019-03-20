using Sketch7.Core.Models;
using System;
using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines that the entity is sortable.
	/// </summary>
	public interface ISortableData<TEntity, TKey> where TEntity : EntityBase
	{
		Expression<Func<TEntity, TKey>> OrderBy { get; set; }
		int PageIndex { get; set; }
		int PageSize { get; set; }
		SortOrder SortOrder { get; set; }
	}
}
