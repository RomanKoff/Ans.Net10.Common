// rev 2026-09-16

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Предоставляет методы для формирования человекочитаемых дат
	/// на основе фиксированного снимка времени (Snapshot).
	/// </summary>
	public class DateTimeHelper
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="DateTimeHelper"/>, фиксируя текущее время.
		/// </summary>
		public DateTimeHelper()
		{
			Current = SuppDateTime.Current;
			CurrentYearBegin = new DateTime(Current.Year, 1, 1);
			NextYearBegin = CurrentYearBegin.AddYears(1);
			Today = Current.Date;
			Yesterday = Today.AddDays(-1);
			Tomorrow = Today.AddDays(1);
			TomorrowAfter = Today.AddDays(2);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает зафиксированные текущие дату и время на момент создания экземпляра.
		/// </summary>
		public DateTime Current { get; }

		/// <summary>
		/// Возвращает дату начала текущего календарного года.
		/// </summary>
		public DateTime CurrentYearBegin { get; }

		/// <summary>
		/// Возвращает дату начала следующего календарного года.
		/// </summary>
		public DateTime NextYearBegin { get; }

		/// <summary>
		/// Возвращает текущую дату без учета времени.
		/// </summary>
		public DateTime Today { get; }

		/// <summary>
		/// Возвращает дату вчерашнего дня.
		/// </summary>
		public DateTime Yesterday { get; }

		/// <summary>
		/// Возвращает дату завтрашнего дня.
		/// </summary>
		public DateTime Tomorrow { get; }

		/// <summary>
		/// Возвращает дату послезавтрашнего дня.
		/// </summary>
		public DateTime TomorrowAfter { get; }


		/* functions */


		/// <summary>
		/// Возвращает человекочитаемую строку с датой и временем события для публикаций.
		/// </summary>
		/// <param name="datetime">Дата и время проверяемого события.</param>
		/// <param name="addTime">Признак необходимости добавления времени к строке результата.</param>
		/// <param name="useYesterdayTodayTomorrow">Признак использования словесных подстановок "сегодня", "вчера", "завтра".</param>
		/// <returns>Форматированная человекочитаемая строка.</returns>
		public string GetPassed(
			DateTime datetime,
			bool addTime,
			bool useYesterdayTodayTomorrow)
		{
			if (addTime && !datetime.HasTimeOfDay())
				addTime = false;

			// Сценарий 1: Далекое будущее (следующие года)
			if (datetime >= NextYearBegin)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_Full
					: Resources.Common.Format_Date_Full);

			// Сценарий 2: Будущее (этот год, начиная с ПОСЛЕЗАВТРА)
			if (datetime >= TomorrowAfter)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_DayAndMonth
					: Resources.Common.Format_Date_DayAndMonth);

			// Сценарий 3: ЗАВТРА
			if (datetime >= Tomorrow)
				return useYesterdayTodayTomorrow
					? datetime.ToString(addTime
						? Resources.Common.Format_DateTime_Tomorrow
						: Resources.Common.Format_Date_Tomorrow)
					: datetime.ToString(addTime
						? Resources.Common.Format_DateTime_DayAndMonth
						: Resources.Common.Format_Date_DayAndMonth);

			// Сценарий 4: СЕГОДНЯ
			if (datetime >= Today)
				return useYesterdayTodayTomorrow
					? datetime.ToString(addTime
						? Resources.Common.Format_DateTime_Today
						: Resources.Common.Format_Date_Today)
					: datetime.ToString(addTime
						? Resources.Common.Format_DateTime_DayAndMonth
						: Resources.Common.Format_Date_DayAndMonth);

			// Сценарий 5: ВЧЕРА
			if (datetime >= Yesterday)
				return useYesterdayTodayTomorrow
					? datetime.ToString(addTime
						? Resources.Common.Format_DateTime_Yesterday
						: Resources.Common.Format_Date_Yesterday)
					: datetime.ToString(addTime
						? Resources.Common.Format_DateTime_DayAndMonth
						: Resources.Common.Format_Date_DayAndMonth);

			// Сценарий 6: Недавнее прошлое (этот год, до ВЧЕРА)
			if (datetime >= CurrentYearBegin)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_DayAndMonth
					: Resources.Common.Format_Date_DayAndMonth);

			// Сценарий 7: Далекое прошлое (предыдущие года)
			return datetime.ToString(addTime
				? Resources.Common.Format_DateTime_Full
				: Resources.Common.Format_Date_Full);
		}


		/// <summary>
		/// Возвращает человекочитаемую строку с датой и временем события с поддержкой значений null.
		/// </summary>
		/// <param name="datetime">Дата и время проверяемого события, допускающие значение null.</param>
		/// <param name="addTime">Признак необходимости добавления времени к строке результата.</param>
		/// <param name="useYesterdayTodayTomorrow">Признак использования словесных подстановок "сегодня", "вчера", "завтра".</param>
		/// <returns>Форматированная строка или текст по умолчанию, если значение не задано.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetPassed(
			DateTime? datetime,
			bool addTime,
			bool useYesterdayTodayTomorrow)
		{
			return datetime.HasValue
				? GetPassed(datetime.Value, addTime, useYesterdayTodayTomorrow)
				: Resources.Common.Text_Never;
		}


		/// <summary>
		/// Возвращает человекочитаемую строку с датой события для публикаций без учета времени.
		/// </summary>
		/// <param name="date">Дата проверяемого события.</param>
		/// <param name="useYesterdayTodayTomorrow">Признак использования словесных подстановок "сегодня", "вчера", "завтра".</param>
		/// <returns>Форматированная человекочитаемая строка.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetPassed(
			DateOnly date,
			bool useYesterdayTodayTomorrow)
		{
			return GetPassed(date.GetDateTime(), false, useYesterdayTodayTomorrow);
		}


		/// <summary>
		/// Возвращает человекочитаемую строку с датой события с поддержкой значений null.
		/// </summary>
		/// <param name="date">Дата проверяемого события, допускающая значение null.</param>
		/// <param name="useYesterdayTodayTomorrow">Признак использования словесных подстановок "сегодня", "вчера", "завтра".</param>
		/// <returns>Форматированная строка или текст по умолчанию, если значение не задано.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetPassed(
			DateOnly? date,
			bool useYesterdayTodayTomorrow)
		{
			return date.HasValue
				? GetPassed(date.Value.GetDateTime(), false, useYesterdayTodayTomorrow)
				: Resources.Common.Text_Never;
		}

	}

}
