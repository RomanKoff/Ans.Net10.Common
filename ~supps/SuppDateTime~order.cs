// rev 2026-09-15

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class SuppDateTime
	{

		/// <summary>
		/// Возвращает наибольшее значение из двух дат.
		/// </summary>
		/// <param name="value1">Первое значение для сравнения.</param>
		/// <param name="value2">Второе значение для сравнения (может быть <see langword="null"/>).</param>
		/// <returns>Максимальная дата.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateTime Max(
			DateTime value1,
			DateTime? value2)
		{
			return SuppValues.MaxValue(value1, value2);
		}


		/// <summary>
		/// Возвращает наибольшее значение из двух дат типа <see cref="DateOnly"/>.
		/// </summary>
		/// <param name="value1">Первое значение для сравнения.</param>
		/// <param name="value2">Второе значение для сравнения (может быть <see langword="null"/>).</param>
		/// <returns>Максимальная дата.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateOnly Max(
			DateOnly value1,
			DateOnly? value2)
		{
			return SuppValues.MaxValue(value1, value2);
		}


		/// <summary>
		/// Возвращает наибольшее значение из двух временных меток типа <see cref="TimeOnly"/>.
		/// </summary>
		/// <param name="value1">Первое значение для сравнения.</param>
		/// <param name="value2">Второе значение для сравнения (может быть <see langword="null"/>).</param>
		/// <returns>Максимальное время.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeOnly Max(
			TimeOnly value1,
			TimeOnly? value2)
		{
			return SuppValues.MaxValue(value1, value2);
		}


		/// <summary>
		/// Возвращает наименьшее значение из двух дат.
		/// </summary>
		/// <param name="value1">Первое значение для сравнения.</param>
		/// <param name="value2">Второе значение для сравнения (может быть <see langword="null"/>).</param>
		/// <returns>Минимальная дата.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateTime Min(
			DateTime value1,
			DateTime? value2)
		{
			return SuppValues.MinValue(value1, value2);
		}


		/// <summary>
		/// Возвращает наименьшее значение из двух дат типа <see cref="DateOnly"/>.
		/// </summary>
		/// <param name="value1">Первое значение для сравнения.</param>
		/// <param name="value2">Второе значение для сравнения (может быть <see langword="null"/>).</param>
		/// <returns>Минимальная дата.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateOnly Min(
			DateOnly value1,
			DateOnly? value2)
		{
			return SuppValues.MinValue(value1, value2);
		}


		/// <summary>
		/// Возвращает наименьшее значение из двух временных меток типа <see cref="TimeOnly"/>.
		/// </summary>
		/// <param name="value1">Первое значение для сравнения.</param>
		/// <param name="value2">Второе значение для сравнения (может быть <see langword="null"/>).</param>
		/// <returns>Минимальное время.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeOnly Min(
			TimeOnly value1,
			TimeOnly? value2)
		{
			return SuppValues.MinValue(value1, value2);
		}

	}

}
