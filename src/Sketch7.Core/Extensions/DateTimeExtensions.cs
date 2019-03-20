using System;
using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Sketch7.Core
{
	/// <summary>
	/// 	Extension methods for DateTime.
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// 	Accepts a date time value, calculates number of days, minutes or seconds and shows 'pretty dates' like '2 days ago', '1 week ago' or '10 minutes ago'
		/// </summary>
		/// <param name="date"> </param>
		/// <returns> </returns>
		public static string ToPrettyDate(this DateTime date)
		{
			// 1.
			// Get time span elapsed since the date.
			var s = DateTime.Now.Subtract(date);

			// 2.
			// Get total number of days elapsed.
			var dayDiff = (int)s.TotalDays;

			// 3.
			// Get total number of seconds elapsed.
			var secDiff = (int)s.TotalSeconds;

			// 4.
			// Don't allow out of range values.
			if (dayDiff < 0 || dayDiff >= 31)
			{
				return date.ToString(CultureInfo.InvariantCulture);
			}

			// 5.
			// Handle same-day times.
			if (dayDiff == 0)
			{
				// A.
				// Less than one minute ago.
				if (secDiff < 60)
				{
					return "just now";
				}
				// B.
				// Less than 2 minutes ago.
				if (secDiff < 120)
				{
					return "1 minute ago";
				}
				// C.
				// Less than one hour ago.
				if (secDiff < 3600)
				{
					return $"{Math.Floor((double)secDiff / 60)} minutes ago";
				}
				// D.
				// Less than 2 hours ago.
				if (secDiff < 7200)
				{
					return "1 hour ago";
				}
				// E.
				// Less than one day ago.
				if (secDiff < 86400)
				{
					return $"{Math.Floor((double)secDiff / 3600)} hours ago";
				}
			}
			// 6.
			// Handle previous days.
			if (dayDiff == 1)
			{
				return "yesterday";
			}
			if (dayDiff < 7)
			{
				return $"{dayDiff} days ago";
			}
			if (dayDiff < 31)
			{
				return $"{Math.Ceiling((double)dayDiff / 7)} weeks ago";
			}
			return null;
		}

		/// <summary>
		/// 	Get actual age of person from DateTime (it must contain the DateOfBirth e.g. 21/07/1988).
		/// </summary>
		/// <param name="dateOfBirth"> Date of birth. </param>
		/// <returns> Return Age as Int32. </returns>
		public static int GetAge(this DateTime dateOfBirth)
		{
			if (DateTime.Today.Month < dateOfBirth.Month
				|| DateTime.Today.Month == dateOfBirth.Month
				&& DateTime.Today.Day < dateOfBirth.Day)
				return DateTime.Today.Year - dateOfBirth.Year - 1;
			return DateTime.Today.Year - dateOfBirth.Year;
		}

		private const string TimeFormat = "HH:mm";

		private const string ShortDateFormat = "dd/MM/yyyy";
		private const string MediumDateFormat = "dd MMM yyyy";
		private const string LongDateFormat = "dd MMMM yyyy";

		#region ToEnDate

		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent short date string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <returns> Return date as string. </returns>
		public static string ToShortEnDateString(this DateTime date) => date.ToString(ShortDateFormat);

		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent short date string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <param name="delimiter"> Delimiter to split string. e.g. 21-07-1988. </param>
		/// <returns> Return date as string. </returns>
		public static string ToShortEnDateString(this DateTime date, char delimiter) => date.ToString(ShortDateFormat).Replace('/', delimiter);

		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent medium date string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <returns> Return date as string. </returns>
		public static string ToMediumEnDateString(this DateTime date) => date.ToString(MediumDateFormat);

		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent long date string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <returns> Return date as string. </returns>
		public static string ToLongEnDateString(this DateTime date) => date.ToString(LongDateFormat);


		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent short date time string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <returns> Return date as string. </returns>
		public static string ToShortEnDateTimeString(this DateTime date) => date.ToString($"{ShortDateFormat} {TimeFormat}");

		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent short date time string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <param name="delimiter"> Delimiter to split string. e.g. 21-07-1988. </param>
		/// <returns> Return date as string. </returns>
		public static string ToShortEnDateTimeString(this DateTime date, char delimiter)
		{
			return date.ToString($"{ShortDateFormat} {TimeFormat}").Replace('/', delimiter);
			;
		}

		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent medium date time string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <returns> Return date as string. </returns>
		public static string ToMediumEnDateTimeString(this DateTime date) => date.ToString($"{MediumDateFormat} {TimeFormat}");

		/// <summary>
		/// 	Converts the value of the current System.DateTime object to its equivalent long date time string representation.
		/// </summary>
		/// <param name="date"> Date value. </param>
		/// <returns> Return date as string. </returns>
		public static string ToLongEnDateTimeString(this DateTime date) => date.ToString($"{LongDateFormat} {TimeFormat}");

		#endregion
	}

	//public enum DatePrettyType
	//{
	//    JustNow,
	//    OneMinuteAgo,
	//    MinutesAgo,
	//    OneHourAgo,
	//    HoursAgo,
	//    Yesterday,
	//    DaysAgo,
	//    WeeksAgo
	//}
}