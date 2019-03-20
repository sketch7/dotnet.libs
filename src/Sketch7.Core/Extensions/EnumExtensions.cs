using System;
using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	public static class EnumExtensions
	{
		/// <summary>
		/// 	Enum Get Description
		/// </summary>
		/// <param name="en"> </param>
		/// <returns> </returns>
		public static string ToDescription(this Enum en)
		{
			var description = string.Empty;

			var memberInfo = en.GetType().GetMember(en.ToString());
			if (memberInfo.Length <= 0)
				return description;

			var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0)
			{
				description = ((DescriptionAttribute)attributes[0]).Description;
			}
			return description;
		}
	}
}