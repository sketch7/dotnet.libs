using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	public interface IEntityAction<TEntity> where TEntity : EntityBase
	{
		EntityState EntityState { get; set; }
		TEntity Entity { get; set; }
	}
}