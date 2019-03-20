// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines that the entity is sortable.
	/// </summary>
	public interface ISortableEntity
	{
		int DisplayOrder { get; set; }
	}
}