using System;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines that the entity has the CreatedDate and LastModified date. These fields will be set when updating or creating in the repository automatically.
	/// </summary>
	public interface IModifiedEntity
	{
		DateTime CreatedDate { get; set; }
		DateTime? LastModified { get; set; }
	}
}