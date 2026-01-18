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



	public class DateTimeHelper
	{

		/* ctor */


		public DateTimeHelper()
		{
			Current = DateTime.Now;
			CurrentYearBegin = new DateTime(Current.Year, 1, 1);
			NextYearBegin = CurrentYearBegin.AddYears(1);
			Today = Current.Date;
			Yesterday = Today.AddDays(-1);
			Tomorrow = Today.AddDays(1);
			TomorrowAfter = Today.AddDays(2);
		}


		/* readonly properties */


		/// <summary>
		/// Текущая дата и время
		/// </summary>
		public DateTime Current { get; }


		/// <summary>
		/// Дата начала текущего года
		/// </summary>
		public DateTime CurrentYearBegin { get; }


		/// <summary>
		/// Дата начала следующего года
		/// </summary>
		public DateTime NextYearBegin { get; }


		/// <summary>
		/// Дата сегодня
		/// </summary>
		public DateTime Today { get; }


		/// <summary>
		/// Дата вчера
		/// </summary>
		public DateTime Yesterday { get; }


		/// <summary>
		/// Дата завтра
		/// </summary>
		public DateTime Tomorrow { get; }


		/// <summary>
		/// Дата послезавтра
		/// </summary>
		public DateTime TomorrowAfter { get; }


		/* functions */


		/// <summary>
		/// Возвращает для указанной даты значение "прошло", "сейчас" или "будет".
		/// </summary>
		public TensesEnum GetDayTenses(
			DateTime datetime)
		{
			var date1 = datetime.Date;
			if (date1 < Today)
				return TensesEnum.Past;
			if (date1 > Today)
				return TensesEnum.Future;
			return TensesEnum.Present;
		}


		public TensesEnum GetDayTenses(
			DateOnly date)
		{
			return GetDayTenses(
				date.GetDateTime());
		}


		/// <summary>
		/// Возвращает дату (и время) события (для блога)
		///	- будет в будущих годах
		///		d MMMM yyyy г.[ в H:mmmm] / MMMM d, yyyy[ at h:mmmm]
		///	- будет в этом году
		///		d MMMM[ в H:mmmm] / MMMM d[ at h:mmmm]
		///	- завтра
		///		завтра[ в H:mmmm] / Tomorrow[ at h:mmmm]
		///	- сегодня
		///		сегодня[ в H:mmmm] / Today[ at h:mmmm]
		///	- вчера
		///		вчера[ в H:mmmm] / Yesterday[ at H:mmmm]
		///	- было в этом году
		///		d MMMM[ в H:mmmm] / MMMM d[ at h:mmmm]
		///	- было в прошлые годы
		///		d MMMM yyyy г.[ в H:mmmm] / MMMM d, yyyy[ at h:mmmm]
		/// </summary>
		/// <param name="datetime"></param>
		/// <param name="addTime">Добавить время</param>
		/// <param name="useYesterdayTodayTomorrow">Использовать «вчера», «сегодня» и «завтра»</param>
		public string GetPassed(
			DateTime datetime,
			bool addTime,
			bool useYesterdayTodayTomorrow)
		{
			if (!datetime.HasDayTime())
				addTime = false;
			// будет в будущих годах
			if (datetime >= NextYearBegin)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_Full
					: Resources.Common.Format_Date_Full);
			// будет в этом году
			if (useYesterdayTodayTomorrow && datetime >= TomorrowAfter)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_DayAndMonth
					: Resources.Common.Format_Date_DayAndMonth);
			// завтра
			if (useYesterdayTodayTomorrow && datetime >= Tomorrow)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_Tomorrow
					: Resources.Common.Format_Date_Tomorrow);
			// сегодня
			if (useYesterdayTodayTomorrow && datetime >= Today)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_Today
					: Resources.Common.Format_Date_Today);
			// вчера
			if (useYesterdayTodayTomorrow && datetime >= Yesterday)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_Yesterday
					: Resources.Common.Format_Date_Yesterday);
			// было в этом году
			if (datetime >= CurrentYearBegin)
				return datetime.ToString(addTime
					? Resources.Common.Format_DateTime_DayAndMonth
					: Resources.Common.Format_Date_DayAndMonth);
			// было в прошлые годы
			return datetime.ToString(addTime
				? Resources.Common.Format_DateTime_Full
				: Resources.Common.Format_Date_Full);
		}


		public string GetPassed(
			DateOnly date,
			bool useYesterdayTodayTomorrow)
		{
			return GetPassed(
				date.GetDateTime(), false, useYesterdayTodayTomorrow);
		}


		/// <summary>
		/// Возвращает дату (и время) прошедшего события (для блогов)
		/// </summary>
		/// <param name="datetime"></param>
		/// <param name="addTime">Добавить время</param>
		/// <param name="useYesterdayTodayTomorrow">Использовать «вчера», «сегодня» и «завтра»</param>
		public string GetPassed(
			DateTime? datetime,
			bool addTime,
			bool useYesterdayTodayTomorrow)
		{
			if (datetime == null)
				return Resources.Common.Text_Never;
			return GetPassed(datetime.Value, addTime, useYesterdayTodayTomorrow);
		}


		public string GetPassed(
			DateOnly? date,
			bool addTime,
			bool useYesterdayTodayTomorrow)
		{
			return GetPassed(
				date?.GetDateTime(), addTime, useYesterdayTodayTomorrow);
		}

	}

}
