using System;
using System.Data.HashFunction.xxHash;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	/// <summary>
	/// 	Extension methods for String.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// 	Returns a copy of this string with the First letter only capitalized. e.g. "the person" will be returned as "The person".
		/// </summary>
		/// <param name="value"> String value to capitalize first letter. </param>
		/// <returns> Returns a first letter capitalized string. </returns>
		public static string ToUppercaseFirst(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			var a = value.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}

		/// <summary>
		/// 	Returns a copy of this string with the casing capitalized. e.g. "the person" will be returned as "The Person".
		/// </summary>
		/// <param name="value"> String value to capitalize. </param>
		/// <returns> Returns a capitalized string. </returns>
		public static string ToUppercaseWords(this string value)
		{
			var array = value.ToCharArray();

			// Handle the first letter in the string.
			if (array.Length >= 1)
			{
				if (char.IsLower(array[0]))
					array[0] = char.ToUpper(array[0]);
			}

			// Scan through the letters, checking for spaces.
			// ... Uppercase the lowercase letters following spaces.
			for (var i = 1; i < array.Length; i++)
			{
				if (array[i - 1] == ' ')
				{
					if (char.IsLower(array[i]))
						array[i] = char.ToUpper(array[i]);
				}
			}
			return new string(array);
		}

		/// <summary>
		/// 	Returns a copy of this string with pascal casing spaces.
		/// 	<example>
		/// 		e.g. "ThereIsABook" will be output as "There Is A Book"
		/// 	</example>
		/// 	.
		/// </summary>
		/// <param name="value"> String value to capitalize. </param>
		/// <returns> Returns a separated with spaces pascal string. </returns>
		public static string ToStringWithSpaces(this string value)
			=> Regex.Replace(
				value,
				"(?<!^)" + // don't match on the first character - never want to place a space here
				"(" +
				"  [A-Z][a-z] |" + // put a space before "Aaaa"
				"  (?<=[a-z])[A-Z] |" + // put a space into "aAAA" before the first capital
				"  (?<![A-Z])[A-Z]$" + // if the last letter is capital, prefix it with a space too
				")",
				" $1",
				RegexOptions.IgnorePatternWhitespace);

		/// <summary>
		/// 	Remove special formatters from string, such as tabs, newlines etc... (\t, \n, \r)
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> Return stripped string value. </returns>
		public static string RemoveSpecialFormatters(this string value) => Regex.Replace(value, @"\t|\n|\r", string.Empty);

		/// <summary>
		/// 	Removes all special characters from the string.
		/// </summary>
		/// <param name="value"> </param>
		/// <returns> Return stripped string value. </returns>
		public static string RemoveSpecialCharacters(this string value)
			=> Regex.Replace(value, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);

		/// <summary>
		/// 	Reverse the string in the opposite direction.
		/// </summary>
		/// <param name="value"> String value to reverse. </param>
		/// <returns> Return reversed string. </returns>
		public static string ToReverseString(this string value)
		{
			var arr = value.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		}

		//public static string IfEmptyThen(this string value, Func<string, string> action)
		//{
		//    if (string.IsNullOrWhiteSpace(value))
		//        return action.Invoke(null);
		//    return value;
		//}


		///<summary>
		///	Do action if current string is empty. e.g. myVar.IfEmptyThen(() => myProp.DoSomething);
		///</summary>
		///<param name="value"> Current string option </param>
		///<param name="action"> </param>
		///<example>
		///	Instead of: if (string.IsNullOrWhiteSpace(appName)) { ...Code here }
		///</example>
		public static void IfEmptyThen(this string value, Action action)
		{
			if (string.IsNullOrWhiteSpace(value))
				action();
		}

		private static readonly IxxHash HashFunction = xxHashFactory.Instance.Create();

		public static async Task<string> ComputeHash(this string text)
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
			{
				var value = await HashFunction.ComputeHashAsync(stream);
				return value.AsBase64String();
			}
		}

		public static string ComputeHashSync(this string text)
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
			{
				var value = HashFunction.ComputeHash(stream);
				return value.AsBase64String();
			}
		}

	}
}