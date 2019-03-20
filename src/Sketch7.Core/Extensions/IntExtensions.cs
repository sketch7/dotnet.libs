// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	/// <summary>
	/// 	Extension methods for int.
	/// </summary>
	public static class IntExtensions
	{
		/// <summary>
		/// 	Indicates whether the value specified is even.
		/// </summary>
		/// <param name="value"> The int to check. </param>
		/// <returns> Returns TRUE if value is even. </returns>
		public static bool IsEven(this int value) => value % 2 == 0;

		/// <summary>
		/// 	Indicates whether the value specified is odd.
		/// </summary>
		/// <param name="value"> The int to check. </param>
		/// <returns> Returns TRUE if value is odd. </returns>
		public static bool IsOdd(this int value) => !IsEven(value);

		/// <summary>
		/// 	Indicates whether the specified System.Int32 object is 0 or a System.Int32.MinValue value.
		/// </summary>
		/// <param name="value"> A System.Int32 reference. </param>
		/// <returns> </returns>
		public static bool IsNullOrEmpty(this int value) => (value == int.MinValue || value == 0);
	}
}