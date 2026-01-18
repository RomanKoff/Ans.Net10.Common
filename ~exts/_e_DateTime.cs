namespace Ans.Net10.Common
{

	public static partial class _e_DateTime
	{

		/* functions */


		/// <summary>
		/// Возвращает дату ближайшего дня начала недели
		/// </summary>
		public static DateTime GetStartOfWeek(
			this DateTime value,
			DayOfWeek startOfWeek)
		{
			int diff1 = (7 + (value.DayOfWeek - startOfWeek)) % 7;
			return value.AddDays(-1 * diff1).Date;
		}


		/// <summary>
		/// Возвращает дату ближайшего дня начала месяца
		/// </summary>
		public static DateTime GetStartOfMonth(
			this DateTime value)
		{
			return new DateTime(value.Year, value.Month, 1);
		}


		/// <summary>
		/// Возвращает количество минут в диапазоне
		/// </summary>
		public static int GetTotalMinutes(
			this TimeSpan value)
		{
			return SuppMath.RoundToInt(Math.Abs(value.TotalMinutes));
		}


		/// <summary>
		/// Сравнивает значения даты и времени с точностью minutesDiff
		/// </summary>
		public static bool IsEqual(
			this DateTime value1,
			DateTime value2,
			int minutesDiff)
		{
			return Math.Abs((value1 - value2).TotalMinutes) < minutesDiff;
		}


		/// <summary>
		/// Определяет наличие данных о времени (часы, минуты, секунды)
		/// </summary>
		public static bool HasDayTime(
			this DateTime value)
		{
			return value.TimeOfDay != _Consts.DAY_START_SPAN;
		}


		/// <summary>
		/// Возвращает значение даты в виде строки
		/// в формате yyyy-0MM-dd
		/// </summary>
		public static string GetAnsDate(
			this DateTime value)
		{
			return value.ToString(
				_Consts.FORMAT_ANS_DATE);
		}


		/// <summary>
		/// Возвращает значение даты и времени в виде строки
		/// в формате yyyy-0MM-dd HH:mmmm:ss
		/// </summary>
		public static string GetAnsDateTime(
			this DateTime value)
		{
			return value.ToString(
				_Consts.FORMAT_ANS_DATETIME);
		}


		/// <summary>
		/// Возвращает значение даты и времени в виде строки
		/// в формате yyyy-0MM-dd_HH-mmmm-ss для именования файлов
		/// </summary>
		public static string GetAnsDateTimeForFile(
			this DateTime value)
		{
			return value.ToString(
				_Consts.FORMAT_ANS_DATETIME_FILE);
		}


		public static DateTime GetDateTime(
			this DateOnly date)
		{
			return date.ToDateTime(TimeOnly.MinValue);
		}


		public static TimeOnly GetTimeOnly(
			this DateTime value)
		{
			return TimeOnly.FromDateTime(value);
		}


		public static DateOnly GetDateOnly(
			this DateTime value)
		{
			return DateOnly.FromDateTime(value);
		}

	}

}
