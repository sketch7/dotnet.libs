using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core.Entity.Models
{
	public abstract class EntityCompositeBase<TEntity> : EntityBase
	{
		public void SetKeys<TKey>(Expression<Func<TEntity, TKey>> keyExpression)
		 => KeyProperties = keyExpression.GetSimplePropertyAccessList().Select(p => p.Single());

		public override sealed IEnumerable<PropertyInfo> KeyProperties { get; protected set; }

		private object[] _keyValues;

		public override sealed object[] KeyValues
		{
			get
			{
				if (_keyValues != null)
					return _keyValues;

				if (KeyProperties == null)
					throw new CompositeKeyNotSetException();

				_keyValues = KeyProperties.Select(keyProperty => keyProperty.GetValue(this, null)).ToArray();

				return _keyValues;
			}
		}

		[ScaffoldColumn(false)]
		public override sealed bool IsKeySet => KeyValues.All(keyValue => !keyValue.IsNullOrDefault());

		[ScaffoldColumn(false)]
		public override sealed bool IsComposite => true;

		public override string ToString()
		{
			var ids = "";
			var counter = 0;
			foreach (var propertyInfo in KeyProperties)
			{
				if (string.IsNullOrEmpty(ids))
					ids = propertyInfo.Name + "=" + KeyValues[counter];
				else
					ids = $"{ids}, {propertyInfo.Name}={KeyValues[counter]}";
				counter++;
			}

			return $"PK=[{ids}]";
		}
	}
}