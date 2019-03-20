// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	public abstract class SortableEntityCompositeBase<TEntity> : EntityCompositeBase<TEntity>, ISortableEntity
	{
		public int DisplayOrder { get; set; }

		public override string ToString() => $"{base.ToString()}, DisplayOrder={DisplayOrder}";
	}
}