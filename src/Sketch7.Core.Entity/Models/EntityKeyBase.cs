using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	/// <summary>
	/// 	Defines an Entity base extended by <see cref="Id" /> property which contains a single Key.
	/// </summary>
	public abstract class EntityKeyBase<TKey> : EntityBase
		where TKey : struct
	{
		/// <summary>
		/// 	Gets or sets the primary key for the entity.
		/// </summary>
		public TKey Id { get; set; }

		private IEnumerable<PropertyInfo> _keyProperties;

		[IgnoreDataMember]
		public override sealed IEnumerable<PropertyInfo> KeyProperties
		{
			get
			{
				if (_keyProperties != null)
					return _keyProperties;

				_keyProperties = new List<PropertyInfo> { GetType().GetProperty("Id") };
				return _keyProperties;
			}
			protected set => _keyProperties = value;
		}

		private object[] _keyValues;

		[IgnoreDataMember]
		public override sealed object[] KeyValues
		{
			get
			{
				if (_keyValues != null)
					return _keyValues;

				_keyValues = new object[] {Id};
				return _keyValues;
			}
		}

		[IgnoreDataMember]
		public override bool IsKeySet
		{
			get
			{
				if (Id is int)
					return System.Convert.ToInt32(Id)  > 0;

				return !Id.IsNullOrDefault();
			}
		}

		[IgnoreDataMember]
		public override sealed bool IsComposite => false;

		public override string ToString() => $"Id={Id}";
	}

	/// <summary>
	/// 	Defines an Entity base extended by property, which defaults the primary key as <see cref="int" /> .
	/// </summary>
	public abstract class EntityKeyBase : EntityKeyBase<int>
	{
		//Use EntityKeyBase<TKey> instead.
		[IgnoreDataMember]
		public override sealed bool IsKeySet => Id > 0;
	}
}