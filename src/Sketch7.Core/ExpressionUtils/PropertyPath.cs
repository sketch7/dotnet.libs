using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sketch7.Core.ExpressionUtils
{
	internal class PropertyPath : IEnumerable<PropertyInfo>, IEnumerable
	{
		public static readonly PropertyPath Empty = new PropertyPath();
		private readonly List<PropertyInfo> _components = new List<PropertyInfo>();

		public int Count => _components.Count;

		public PropertyInfo this[int index] => _components[index];

		static PropertyPath()
		{
		}

		public PropertyPath(IEnumerable<PropertyInfo> components)
		{
			_components.AddRange(components);
		}

		public PropertyPath(PropertyInfo component)
		{
			_components.Add(component);
		}

		private PropertyPath()
		{
		}

		public static bool operator ==(PropertyPath left, PropertyPath right) => Equals((object)left, (object)right);

		public static bool operator !=(PropertyPath left, PropertyPath right) => !Equals((object)left, (object)right);

		public override string ToString()
		{
			var propertyPathName = new StringBuilder();
			_components.Each(pi =>
			{
				propertyPathName.Append(pi.Name);
				propertyPathName.Append('.');
			});
			return propertyPathName.ToString(0, propertyPathName.Length - 1);
		}

		public bool Equals(PropertyPath other)
		{
			if (ReferenceEquals((object)null, (object)other))
				return false;
			if (ReferenceEquals((object)this, (object)other))
				return true;

			return _components.SequenceEqual(other._components, (p1, p2) => p1.IsSameAs(p2));
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals((object)null, obj))
				return false;
			if (ReferenceEquals((object)this, obj))
				return true;
			if (obj.GetType() != typeof(PropertyPath))
				return false;

			return Equals((PropertyPath)obj);
		}

		public override int GetHashCode() => _components.Aggregate(0, (t, n) => t + n.GetHashCode());

		IEnumerator<PropertyInfo> IEnumerable<PropertyInfo>.GetEnumerator() => _components.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _components.GetEnumerator();
	}
}