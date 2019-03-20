using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	public class EntityAction<TEntity> : IEntityAction<TEntity> where TEntity : EntityBase
	{
		public EntityState EntityState { get; set; }
		public TEntity Entity { get; set; }
	}
}