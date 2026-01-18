namespace Ans.Net10.Common
{

	public static class SuppDateTime
	{

		/* functions */


		/// <summary>
		/// Возвращает диапазон дат (для блога)
		/// - одна или равные даты
		/// - разные года
		/// - разные месяцы года
		/// - разные дни месяца
		/// </summary>
		/// <param name="showCurrentYear">Отображать текущий год</param>
		public static string GetSpan(
			DateTime date1,
			DateTime? date2,
			bool showCurrentYear)
		{
			// одна или равные даты
			if (date2 == null || date1.Date.Equals(date2.Value.Date))
				return (showCurrentYear)
					? date1.ToString(Resources.Common.Format_Date_Full)
					: date1.ToString(Resources.Common.Format_Date_DayAndMonth);
			DateTime d2 = date2.Value;
			if (date1 > d2)
				(date1, d2) = (d2, date1);
			if (date1.Year != d2.Year)
			{
				// разные года
				return string.Format("{0} – {1}",
					date1.ToString(Resources.Common.Format_Date_Full),
					d2.ToString(Resources.Common.Format_Date_Full));
			}
			if (date1.Month != d2.Month)
			{
				// разные месяцы года
				return string.Format("{0} – {1}",
					date1.ToString(Resources.Common.Format_Date_DayAndMonth),
					(showCurrentYear)
						? d2.ToString(Resources.Common.Format_Date_Full)
						: d2.ToString(Resources.Common.Format_Date_DayAndMonth));
			}
			// разные дни месяца
			return string.Format("{0}–{1}",
				date1.ToString(Resources.Common.Format_Date_Day),
				(showCurrentYear)
					? d2.ToString(Resources.Common.Format_Date_Full)
					: d2.ToString(Resources.Common.Format_Date_DayAndMonth));
		}


		/// <summary>
		/// Возвращает диапазон дат (для блога)
		/// </summary>
		/// <param name="showCurrentYear">Отображать текущий год</param>
		public static string GetSpan(
			DateOnly date1,
			DateOnly? date2,
			bool showCurrentYear)
		{
			return GetSpan(
				date1.GetDateTime(),
				date2?.GetDateTime(),
				showCurrentYear);
		}


		/// <summary>
		/// Возвращает диапазон дат (для блога)
		/// </summary>
		/// <param name="span">
		/// Строка диапазона дат в формате:
		/// "datetime1_datetime2"
		/// </param>
		/// <param name="showCurrentYear">Отображать текущий год</param>
		public static string GetSpan(
			string span,
			bool showCurrentYear)
		{
			var a1 = span.SplitFix("|", 2);
			var d1 = a1[0].ToDateTime();
			if (d1 == null)
				return null;
			var d2 = a1[1].ToDateTime();
			return GetSpan(d1.Value, d2, showCurrentYear);
		}


		/// <summary>
		/// Возвращает коллекцию дней от start до end
		/// </summary>
		public static IEnumerable<DateTime> GetDays(
			DateTime start,
			DateTime end)
		{
			var a1 = Enumerable.Range(0, end.Date.Subtract(start.Date).Days + 1)
				.Select(x => start.Date.AddDays(x));
			return [.. a1];
		}


		/// <summary>
		/// Возвращает коллекцию дней от start до end
		/// </summary>
		public static IEnumerable<DateTime> GetDays(
			DateOnly start,
			DateOnly end)
		{
			return GetDays(
				start.GetDateTime(),
				end.GetDateTime());
		}


		public static DateTime Max(
			DateTime value1,
			DateTime? value2)
		{
			if (value2 == null)
				return value1;
			return (value1 > value2) ? value1 : value2.Value;
		}


		public static DateOnly Max(
			DateOnly value1,
			DateOnly? value2)
		{
			if (value2 == null)
				return value1;
			return (value1 > value2) ? value1 : value2.Value;
		}


		public static TimeOnly Max(
			TimeOnly value1,
			TimeOnly? value2)
		{
			if (value2 == null)
				return value1;
			return (value1 > value2) ? value1 : value2.Value;
		}


		public static DateTime Min(
			DateTime value1,
			DateTime? value2)
		{
			if (value2 == null)
				return value1;
			return (value1 < value2) ? value1 : value2.Value;
		}


		public static DateOnly Min(
			DateOnly value1,
			DateOnly? value2)
		{
			if (value2 == null)
				return value1;
			return (value1 < value2) ? value1 : value2.Value;
		}


		public static TimeOnly Min(
			TimeOnly value1,
			TimeOnly? value2)
		{
			if (value2 == null)
				return value1;
			return (value1 < value2) ? value1 : value2.Value;
		}


		public static DateTime GetDateTimeFromUnixTimeStamp(
			double value)
		{
			// Unix timestamp is seconds past epoch
			var date1 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			date1 = date1.AddSeconds(value).ToLocalTime();
			return date1;
		}
		
		
		public static DateTime? GetDateTimeFromUnixTimeStamp(
			double? value)
		{
			// Unix timestamp is seconds past epoch
			return value == null
				? null
				: GetDateTimeFromUnixTimeStamp(value.Value);
		}


		public static DateTime GetDateTimeFromJavaTimeStamp(
			double value)
		{
			// Java timestamp is milliseconds past epoch
			var date1 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			date1 = date1.AddMilliseconds(value).ToLocalTime();
			return date1;
		}
		
		
		public static DateTime? GetDateTimeFromJavaTimeStamp(
			double? value)
		{
			// Java timestamp is milliseconds past epoch
			return value == null
				? null
				: GetDateTimeFromJavaTimeStamp(value.Value);
		}


		/// <summary>
		/// Возвращает DateTime из короткой строки даты в формате
		/// YYYY-MM-DD
		/// </summary>
		public static DateTime? GetDateFromUniDate(
			string value)
		{
			if (value.Length != 10)
				return null;
			try
			{
				var year1 = value[..4].ToInt(0);
				var month1 = value[5..7].ToInt(0);
				var day1 = value[8..10].ToInt(0);
				return new DateTime(year1, month1, day1);
			}
			catch (Exception)
			{
				return null;
			}
		}


		/// <summary>
		/// Возвращает DateTime из короткой строки даты в формате
		/// YYYY-0MM-DD
		/// </summary>
		public static DateTime? GetDateFromAnsDate(
			string value)
		{
			if (value.Length != 10)
				return null;
			try
			{
				var year1 = value[..4].ToInt(0);
				var month1 = value[6..8].ToInt(0);
				var day1 = value[9..11].ToInt(0);
				return new DateTime(year1, month1, day1);
			}
			catch (Exception)
			{
				return null;
			}
		}

	}

}
