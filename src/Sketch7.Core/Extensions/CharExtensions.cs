// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	/// <summary>
	/// 	Extension methods for String.
	/// </summary>
	public static class CharExtensions
	{
		/// <summary>
		/// 	Indicates whether the char specified is vowel.
		/// </summary>
		/// <param name="letter"> Letter to check. </param>
		/// <returns> Returns TRUE if letter is vowel. </returns>
		public static bool IsVowel(this char letter)
		{
			letter = char.ToLower(letter);
			switch (letter)
			{
				case 'a':
				case 'e':
				case 'i':
				case 'o':
				case 'u':
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// 	Indicates whether the char specified is consonant.
		/// </summary>
		/// <param name="letter"> Letter to check. </param>
		/// <returns> Returns TRUE if letter is consonant. </returns>
		public static bool IsConsonants(this char letter) => !IsVowel(letter);
	}
}