// rev 2026-09-19

namespace Ans.Net10.Common
{

	/// <summary>
	/// Хелпер периодического интервального таймера для отслеживания моментов наступления событий.
	/// </summary>
	public class TinyTimerHelper
	{

		/* ctor */

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TinyTimerHelper"/> и вычисляет метку первого стопа.
		/// </summary>
		/// <param name="span">Интервал времени между событиями.</param>
		/// <param name="useEqualIntervals">
		/// Если <see langword="true"/>, шаг следующего стопа строго прибавляется
		/// к предыдущему плановому времени (сетка). 
		/// Если <see langword="false"/>, следующий шаг рассчитывается
		/// от фактического момента вызова метода проверки.
		/// </param>
		public TinyTimerHelper(
			TimeSpan span,
			bool useEqualIntervals)
		{
			Span = span;
			UseEqualIntervals = useEqualIntervals;
			NextStop = DateTime.Now + Span;
		}


		/* properties */


		/// <summary>
		/// Возвращает или задает интервал времени между событиями.
		/// </summary>
		public TimeSpan Span { get; set; }


		/// <summary>
		/// Возвращает или задает временную метку следующего запланированного срабатывания таймера.
		/// </summary>
		public DateTime NextStop { get; set; }


		/// <summary>
		/// Возвращает или задает признак использования строгой фиксированной сетки интервалов.
		/// </summary>
		public bool UseEqualIntervals { get; set; }


		/* functions */


		/// <summary>
		/// Проверяет, наступило ли запланированное время. Если время пришло,
		/// автоматически сдвигает метку следующего стопа.
		/// </summary>
		/// <returns><see langword="true"/>, если интервал времени пройден
		/// и событие наступило; иначе — <see langword="false"/>.</returns>
		public bool Test()
		{
			var now1 = DateTime.Now;
			if (NextStop >= now1)
				return false;
			NextStop = UseEqualIntervals
				? NextStop + Span
				: now1 + Span;
			return true;
		}

	}

}
