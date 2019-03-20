// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	/// <summary>
	/// 	Extension methods for int.
	/// </summary>
	public static class LongExtensions
	{
		/// <summary>
		/// 	Indicates whether the value specified is even.
		/// </summary>
		/// <param name="value"> The int to check. </param>
		/// <returns> Returns TRUE if value is even. </returns>
		public static bool IsEven(this long value) => (value % 2) == 0;

		/// <summary>
		/// 	Indicates whether the value specified is odd.
		/// </summary>
		/// <param name="value"> The int to check. </param>
		/// <returns> Returns TRUE if value is odd. </returns>
		public static bool IsOdd(this long value) => !IsEven(value);

		/// <summary>
		/// 	Indicates whether the specified System.Int64 object is 0 or a System.Int64.MinValue value.
		/// </summary>
		/// <param name="value"> A System.Int64 reference. </param>
		/// <returns> Returns True if empty. </returns>
		public static bool IsNullOrEmpty(this long value) => (value == long.MinValue || value == 0);
	}
}