// rev 2026-09-15

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class __e_datetime
	{

		/*--- DateTime ---*/


		/// <summary>
		/// Преобразует текущий объект <see cref="DateTime"/> в объект <see cref="TimeOnly"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeOnly GetTimeOnly(
			this DateTime instance)
		{
			return TimeOnly.FromDateTime(instance);
		}


		/// <summary>
		/// Преобразует текущий объект <see cref="DateTime"/> в объект <see cref="DateOnly"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateOnly GetDateOnly(
			this DateTime instance)
		{
			return DateOnly.FromDateTime(instance);
		}


		/// <summary>
		/// Определяет временную эпоху для указанной даты относительно сегодняшнего дня.
		/// </summary>
		public static TensesEnum GetTenses(
			this DateTime instance)
		{
			var today1 = DateTime.Now.Date;
			var date1 = instance.Date;
			if (date1 < today1)
				return TensesEnum.Past;
			return date1 > today1
				? TensesEnum.Future : TensesEnum.Present;
		}


		/// <summary>
		/// Возвращает дату начала недели для указанной даты с учетом заданного дня начала недели.
		/// </summary>
		public static DateTime GetStartOfWeek(
			this DateTime instance,
			DayOfWeek startOfWeek)
		{
			int diff1 = (7 + (instance.DayOfWeek - startOfWeek)) % 7;
			return instance.AddDays(-1 * diff1).Date;
		}


		/// <summary>
		/// Возвращает дату начала календарного месяца (1-е число) для указанной даты.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateTime GetStartOfMonth(
			this DateTime instance)
		{
			return new DateTime(instance.Year, instance.Month, 1);
		}


		/// <summary>
		/// Проверяет равенство двух дат с точностью до указанного количества минут.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEqual(
			this DateTime instance,
			DateTime value2,
			int minutesDiff)
		{
			return Math.Abs((instance - value2).TotalMinutes) <= minutesDiff;
		}


		/// <summary>
		/// Проверяет, содержит ли текущий объект <see cref="DateTime"/>
		/// время суток (отличное от полночи 00:00:00).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasTimeOfDay(
			this DateTime instance)
		{
			return instance.TimeOfDay != TimeSpan.Zero;
		}


		/// <summary>
		/// Возвращает строковое представление даты
		/// в специализированном формате "yyyy-'0'MM-dd".
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetAnsDate(
			this DateTime instance)
		{
			return instance.ToString(SuppDateTime.ANS_DATE_FORMAT);
		}


		/// <summary>
		/// Возвращает строковое представление даты и времени
		/// в специализированном формате "yyyy-'0'MM-dd HH:mm:ss".
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetAnsDateTime(
			this DateTime instance)
		{
			return instance.ToString(SuppDateTime.ANS_DATETIME_FORMAT);
		}


		/// <summary>
		/// Возвращает строковое представление даты и времени,
		/// адаптированное для имени файла.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetAnsDateTimeForFile(
			this DateTime instance)
		{
			return instance.ToString(SuppDateTime.ANS_DATETIME_FILE_FORMAT);
		}


		/*--- DateOnly ---*/


		/// <summary>
		/// Преобразует объект <see cref="DateOnly"/> в тип <see cref="DateTime"/>
		/// на начало суток (00:00:00).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateTime GetDateTime(
			this DateOnly instance)
		{
			return instance.ToDateTime(TimeOnly.MinValue);
		}


		/// <summary>
		/// Определяет временную эпоху для объекта <see cref="DateOnly"/>
		/// относительно сегодняшнего дня.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TensesEnum GetTenses(
			this DateOnly instance)
		{
			return instance.GetDateTime().GetTenses();
		}


		/*--- TimeSpan ---*/


		/// <summary>
		/// Возвращает общее количество минут во временном интервале,
		/// округленное до целого числа.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetTotalMinutes(
			this TimeSpan instance)
		{
			return SuppMath.RoundToInt(Math.Abs(instance.TotalMinutes));
		}

	}

}
