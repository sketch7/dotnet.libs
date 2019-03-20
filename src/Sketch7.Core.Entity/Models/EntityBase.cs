using System.Collections.Generic;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines an Entity base for the entities. This is usually used to provide support for Composite Primary Keys entities, if it isn't needed use <see
	///  	cref="EntityKeyBase" />
	/// </summary>
	public abstract class EntityBase : ModelBase
	{
		/// <summary>
		/// 	The values of the primary key for the entity to be found.
		/// </summary>
		public abstract object[] KeyValues { get; }

		public abstract IEnumerable<PropertyInfo> KeyProperties { get; protected set; }


		public abstract bool IsKeySet { get; }

		/// <summary>
		/// 	Determine whether it has composite key.
		/// </summary>
		public abstract bool IsComposite { get; }
	}
}