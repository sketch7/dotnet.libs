using System;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines an Entity base extended by CreatedDate and LastModified.
	/// </summary>
	public abstract class EntityModifiedKeyBase<TKey> : EntityKeyBase<TKey>, IModifiedEntity
		where TKey : struct
	{
		public DateTime CreatedDate { get; set; }
		public DateTime? LastModified { get; set; }

		public override string ToString() => $"{base.ToString()}, CreatedDate={CreatedDate}, LastModified={LastModified}";
	}

	public abstract class EntityModifiedKeyBase : EntityModifiedKeyBase<int>
	{
		//Use EntityModifiedKeyBase<TKey> instead.
	}
}