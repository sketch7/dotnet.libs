using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	internal static class EnumerableExtensions
	{
		public static void Each<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			var num = 0;
			foreach (var obj in source)
				action(obj, num++);
		}

		public static void Each<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (var obj in source)
				action(obj);
		}

		public static void Each<T, S>(this IEnumerable<T> source, Func<T, S> action)
		{
			foreach (var obj in source)
			{
				var s = action(obj);
			}
		}

		public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, object> uniqueCheckerMethod)
			=> source.Distinct(new GenericComparer<T>(uniqueCheckerMethod));

		private class GenericComparer<T> : IEqualityComparer<T>
		{
			public GenericComparer(Func<T, object> uniqueCheckerMethod) => _uniqueCheckerMethod = uniqueCheckerMethod;

			private readonly Func<T, object> _uniqueCheckerMethod;

			bool IEqualityComparer<T>.Equals(T x, T y) => _uniqueCheckerMethod(x).Equals(_uniqueCheckerMethod(y));

			int IEqualityComparer<T>.GetHashCode(T obj) => _uniqueCheckerMethod(obj).GetHashCode();
		}
	}
}