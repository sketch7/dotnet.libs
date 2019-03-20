using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	public abstract class HierarchyEntityKeyBase<TEntity> : HierarchyEntityKeyBase<TEntity, int>
		where TEntity : EntityBase
	{
		//Use HierarchyEntityKeyBase<TParentKey> instead.
	}

	public abstract class HierarchyEntityGuidKeyBase<TEntity> : HierarchyEntityKeyBase<TEntity, Guid>
		where TEntity : EntityBase
	{
		//Use HierarchyEntityKeyBase<TParentKey> instead.
	}

	public abstract class HierarchyEntityKeyBase<TEntity, TKey> : SortableEntityKeyBase<TKey>,
																  IHierarchyEntity<TEntity, TKey>
		where TEntity : EntityBase
		where TKey : struct
	{
		public HierarchyEntityKeyBase()
		{
			Children = new HashSet<TEntity>();
		}

		public string Name { get; set; }

		public TKey? ParentId { get; set; }

		public byte Level { get; set; }

		public virtual TEntity Parent { get; set; }
		public virtual ICollection<TEntity> Children { get; set; }

		public override string ToString() => $"{base.ToString()}, ParentId={ParentId}, Level={Level}";
	}
}