using System;
using System.Collections.Generic;
using System.Linq;

namespace Sketch7.Core.ExpressionUtils
{
	internal static class DynamicEqualityComparerLinqIntegration
	{
		public static IEnumerable<TItem> Distinct<TItem>(this IEnumerable<TItem> source, Func<TItem, TItem, bool> func) where TItem : class
			=> source.Distinct(new DynamicEqualityComparer<TItem>(func));

		public static IEnumerable<T> Intersect<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> func)
			where T : class => first.Intersect(second, new DynamicEqualityComparer<T>(func));

		public static IEnumerable<IGrouping<TSource, TSource>> GroupBy<TSource>(this IEnumerable<TSource> source,
																				Func<TSource, TSource, bool> func)
			where TSource : class => source.GroupBy(t => t,
														(IEqualityComparer<TSource>)new DynamicEqualityComparer<TSource>(func));

		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other,
												  Func<TSource, TSource, bool> func) where TSource : class
			=> source.SequenceEqual(other, new DynamicEqualityComparer<TSource>(func));
	}
}