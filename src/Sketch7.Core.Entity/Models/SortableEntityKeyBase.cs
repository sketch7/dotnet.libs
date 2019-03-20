// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	public abstract class SortableEntityKeyBase<TKey> : EntityKeyBase<TKey>, ISortableEntity where TKey : struct
	{
		/// <summary>
		/// 	Gets or sets the Display Order.
		/// </summary>
		public int DisplayOrder { get; set; }

		public override string ToString() => $"{base.ToString()}, DisplayOrder={DisplayOrder}";
	}

	public abstract class SortableEntityKeyBase : SortableEntityKeyBase<int>
	{
		//Use SortableEntityKeyBase<TKey> instead.
	}
}