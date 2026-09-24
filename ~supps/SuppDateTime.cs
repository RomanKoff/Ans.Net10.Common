// rev 2026-09-15

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public enum TensesEnum
	{
		/// <summary>Прошло</summary>
		Past,

		/// <summary>Сейчас</summary>
		Present,

		/// <summary>Будет</summary>
		Future
	}



	public static partial class SuppDateTime
	{

		public static readonly Dictionary<string, string> RTC3339FORMATS
			= new(StringComparer.Ordinal) {
				{ "date", "{0:yyyy-MM-dd}" },
				{ "datetime", @"{0:yyyy-MM-ddTHH\:mm\:ss.fffK}" },
				{ "datetime-local", @"{0:yyyy-MM-ddTHH\:mm\:ss.fff}" },
				{ "time", @"{0:HH\:mm\:ss.fff}" },
			};

		public const string ANS_DATETIME_FORMAT = "yyyy-'0'MM-dd HH:mm:ss";
		public const string ANS_DATE_FORMAT = "yyyy-'0'MM-dd";
		public const string ANS_DATETIME_FILE_FORMAT = "yyyy-'0'MM-dd_HH-mm-ss";


		/// <summary>
		/// Получает текущую системную дату и время.
		/// </summary>
		public static DateTime Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => DateTime.Now;
		}


		/// <summary>
		/// Получает дату текущего дня без времени.
		/// </summary>
		public static DateTime Today
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Current.Date;
		}


		/// <summary>
		/// Получает дату начала текущего календарного года.
		/// </summary>
		public static DateTime CurrentYearBegin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new(Current.Year, 1, 1);
		}


		/// <summary>
		/// Получает дату начала следующего календарного года.
		/// </summary>
		public static DateTime NextYearBegin
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => CurrentYearBegin.AddYears(1);
		}


		/// <summary>
		/// Получает дату вчерашнего дня.
		/// </summary>
		public static DateTime Yesterday
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Today.AddDays(-1);
		}


		/// <summary>
		/// Получает дату завтрашнего дня.
		/// </summary>
		public static DateTime Tomorrow
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Today.AddDays(1);
		}


		/// <summary>
		/// Получает дату послезавтрашнего дня.
		/// </summary>
		public static DateTime TomorrowAfter
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Today.AddDays(2);
		}

	}

}
