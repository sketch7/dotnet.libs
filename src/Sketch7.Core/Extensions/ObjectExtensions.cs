// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// 	If T is a reference type, value will be compared with null ( default(T) ), otherwise, if T is a value type, let's say double, default(T) is 0d, for bool is false, for char is '\0' and so on...
		/// </summary>
		/// <typeparam name="T"> Type of the return object. </typeparam>
		/// <param name="value"> value to be validated. </param>
		/// <returns> Returns true if is null else false. </returns>
		public static bool IsNullOrDefault<T>(this T value) => value.Equals(default(T));
	}
}