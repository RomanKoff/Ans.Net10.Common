// rev 2026-09-15

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class SuppDateTime
	{

		private static readonly string[] _SEP_DATES = ["|", "_", ":"];


		/// <summary>
		/// Возвращает диапазон дат, отформатированный для публикации в блоге.
		/// </summary>
		/// <param name="date1">Начальная дата диапазона.</param>
		/// <param name="date2">Конечная дата диапазона (опционально).</param>
		/// <param name="showCurrentYear">Признак принудительного вывода года, даже если он совпадает с текущим.</param>
		/// <returns>Форматированная строка диапазона дат для блога.</returns>
		public static string GetSpan(
			DateTime date1,
			DateTime? date2,
			bool showCurrentYear)
		{
			// Если date2 не задан или даты равны — обрабатываем как одну дату
			if (date2 == null || date1.Date.Equals(date2.Value.Date))
			{
				bool needYear1 = showCurrentYear || date1.Year != DateTime.Today.Year;
				return needYear1
					? date1.ToString(Resources.Common.Format_Date_Full)
					: date1.ToString(Resources.Common.Format_Date_DayAndMonth);
			}
			DateTime d2 = date2.Value;
			if (date1 > d2)
				(date1, d2) = (d2, date1);
			bool needEndYear1 = showCurrentYear || d2.Year != DateTime.Today.Year;

			// Сценарий А: Разные года
			if (date1.Year != d2.Year)
				return $"{date1.ToString(Resources.Common.Format_Date_Full)} – {d2.ToString(Resources.Common.Format_Date_Full)}";

			// Сценарий Б: Год один, но разные месяцы
			if (date1.Month != d2.Month)
			{
				var endDateFormat1 = needEndYear1
					? Resources.Common.Format_Date_Full
					: Resources.Common.Format_Date_DayAndMonth;
				return $"{date1.ToString(Resources.Common.Format_Date_DayAndMonth)} – {d2.ToString(endDateFormat1)}";
			}

			// Сценарий В: Разные дни одного и того же месяца
			var lastDateFormat1 = needEndYear1
				? Resources.Common.Format_Date_Full
				: Resources.Common.Format_Date_DayAndMonth;
			return $"{date1.ToString(Resources.Common.Format_Date_Day)}–{d2.ToString(lastDateFormat1)}";
		}


		/// <summary>
		/// Возвращает диапазон дат (для блога) на основе типов <see cref="DateOnly"/>.
		/// </summary>
		/// <param name="date1">Начальная дата диапазона.</param>
		/// <param name="date2">Конечная дата диапазона (опционально).</param>
		/// <param name="showCurrentYear">Признак принудительного вывода года.</param>
		/// <returns>Форматированная строка диапазона дат.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		/// Возвращает диапазон дат из строки формата "дата1|дата2" или "дата1_дата2".
		/// </summary>
		/// <param name="span">Строка с диапазоном дат и разделителем.</param>
		/// <param name="showCurrentYear">Признак принудительного вывода года.</param>
		/// <returns>Форматированная строка диапазона или <see langword="string.Empty"/>, если парсинг не удался.</returns>
		public static string GetSpan(
			string span,
			bool showCurrentYear,
			IFormatProvider? provider = null)
		{
			if (string.IsNullOrEmpty(span))
				return string.Empty;
			string[]? tokens1 = null;
			foreach (var sep1 in _SEP_DATES)
				if (span.Contains(sep1))
				{
					tokens1 = span.SplitFix(sep1, 2);
					break;
				}
			tokens1 ??= [span, string.Empty];
			var d1 = tokens1[0].ToDateTime(provider);
			if (d1 == null)
				return string.Empty;
			var d2 = tokens1[1].ToDateTime(provider);
			return GetSpan(d1.Value, d2, showCurrentYear);
		}

	}

}
