// rev 2026-09-19

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Легковесный счетчик-прерыватель итераций для периодического выполнения кода внутри тяжелых циклов.
	/// </summary>
	/// <param name="step">Количество шагов (итераций) между срабатываниями триггера.</param>
	public class TinyBreakerHelper(
		int step)
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TinyBreakerHelper"/>
		/// со значением шага по умолчанию (1234).
		/// </summary>
		public TinyBreakerHelper()
			: this(1234)
		{
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает заданный шаг (интервал) срабатывания прерывателя.
		/// </summary>
		public int Step { get; } = step;


		/// <summary>
		/// Возвращает текущее количество накопленных итераций.
		/// </summary>
		public int Current { get; private set; }


		/* functions */


		/// <summary>
		/// Фиксирует следующую итерацию. Возвращает <see langword="true"/>,
		/// если достигнут заданный шаг интервала.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> при достижении лимита шага; иначе — <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Next()
		{
			Current++;
			if (Current < Step)
				return false;
			Current = 0;
			return true;
		}

	}

}
