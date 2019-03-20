using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	internal static class PropertyInfoExtensions
	{
		public static bool IsSameAs(this PropertyInfo propertyInfo, PropertyInfo otherPropertyInfo)
		{
			if (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType)
				return propertyInfo.Name == otherPropertyInfo.Name;
			return false;
		}
	}
}