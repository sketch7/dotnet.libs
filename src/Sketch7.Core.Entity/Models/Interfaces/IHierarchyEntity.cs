using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines that the entity is hierarchy. ParentId is 'TParentKey' .
	/// </summary>
	/// <typeparam name="TParentKey"> The type of the ParentId key. </typeparam>
	/// <typeparam name="TEntity"> The type of the entity. </typeparam>
	public interface IHierarchyEntity<TEntity, TParentKey>
		where TParentKey : struct
		where TEntity : EntityBase
	{
		string Name { get; set; }

		TParentKey? ParentId { get; set; }
		byte Level { get; set; }

		TEntity Parent { get; set; }
		ICollection<TEntity> Children { get; set; }
	}
}