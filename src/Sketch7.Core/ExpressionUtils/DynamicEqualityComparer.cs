using System;
using System.Collections.Generic;

namespace Sketch7.Core.ExpressionUtils
{
	internal sealed class DynamicEqualityComparer<T> : IEqualityComparer<T> where T : class
	{
		private readonly Func<T, T, bool> _func;

		public DynamicEqualityComparer(Func<T, T, bool> func)
		{
			_func = func;
		}

		public bool Equals(T x, T y) => _func(x, y);

		public int GetHashCode(T obj) => 0;
	}
}