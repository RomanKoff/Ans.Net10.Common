// rev 2026-09-16

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Предоставляет обобщенные и оптимизированные математические функции общего назначения.
	/// </summary>
	public static class SuppMath
	{

		/// <summary>
		/// Удерживает значение в заданных пределах (аналог T.Clamp), используя операторы сравнения.
		/// </summary>
		/// <typeparam name="T">Тип сравниваемых данных, поддерживающий операторы сравнения.</typeparam>
		/// <param name="value">Проверяемое значение.</param>
		/// <param name="minLimit">Минимально допустимая граница.</param>
		/// <param name="maxLimit">Максимально допустимая граница.</param>
		/// <returns>Ограниченное значение, не выходящее за рамки указанных лимитов.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetRestrict<T>(
			T value,
			T minLimit,
			T maxLimit)
			where T : IComparisonOperators<T, T, bool>
		{
			if (value < minLimit)
				return minLimit;
			return value > maxLimit ? maxLimit : value;
		}


		/// <summary>
		/// Округляет число с плавающей запятой до ближайшего целого значения <see cref="int"/>.
		/// </summary>
		/// <typeparam name="T">Тип числа, реализующий интерфейс <see cref="IFloatingPoint{T}"/>.</typeparam>
		/// <param name="value">Исходное значение для округления.</param>
		/// <returns>Округленное значение, приведенное к типу <see cref="int"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RoundToInt<T>(
			T value)
			where T : IFloatingPoint<T>
		{
			T rounded1 = T.Round(value, MidpointRounding.AwayFromZero);
			return int.CreateChecked(rounded1);
		}


		/// <summary>
		/// Округляет число с плавающей запятой до ближайшего целого значения <see cref="uint"/>. 
		/// Если значение отрицательное, возвращает 0.
		/// </summary>
		/// <typeparam name="T">Тип числа, реализующий интерфейс <see cref="IFloatingPoint{T}"/>.</typeparam>
		/// <param name="value">Исходное значение для округления.</param>
		/// <returns>Округленное значение, приведенное к типу <see cref="uint"/>, либо 0.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint RoundToUInt<T>(
			T value)
			where T : IFloatingPoint<T>
		{
			if (value < T.Zero)
				return 0;
			T rounded1 = T.Round(value, MidpointRounding.AwayFromZero);
			return uint.CreateChecked(rounded1);
		}


		/// <summary>
		/// Пропорционально переносит (маппит) значение из одного числового диапазона в другой для типов с плавающей запятой.
		/// </summary>
		/// <typeparam name="T">Тип числа, реализующий интерфейс <see cref="IFloatingPoint{T}"/>.</typeparam>
		/// <param name="value">Исходное значение.</param>
		/// <param name="fromMin">Нижняя граница исходного диапазона.</param>
		/// <param name="fromMax">Верхняя граница исходного диапазона.</param>
		/// <param name="toMin">Нижняя граница целевого диапазона.</param>
		/// <param name="toMax">Верхняя граница целевого диапазона.</param>
		/// <param name="useCrop">Если <see langword="true"/>, результат будет жестко ограничен рамками целевого диапазона.</param>
		/// <returns>Преобразованное число в целевом диапазоне.</returns>
		public static T Map<T>(
			T value,
			T fromMin,
			T fromMax,
			T toMin,
			T toMax,
			bool useCrop)
			where T : IFloatingPoint<T>
		{
			if (fromMax == fromMin)
				return toMin;
			T normal1 = (value - fromMin) / (fromMax - fromMin);
			T abs1 = (toMax - toMin) * normal1;
			T result1 = abs1 + toMin;
			if (useCrop)
			{
				T min1 = T.Min(toMin, toMax);
				T max1 = T.Max(toMin, toMax);
				return GetRestrict(result1, min1, max1);
			}
			return result1;
		}


		/// <summary>
		/// Пропорционально переносит (маппит) значение типа <see cref="int"/> из одного диапазона в другой с округлением результата.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Map(
			int value,
			int fromMin,
			int fromMax,
			int toMin,
			int toMax,
			bool useCrop)
		{
			double value1 = Map((double)value, fromMin, fromMax, toMin, toMax, useCrop);
			return RoundToInt(value1);
		}


		/// <summary>
		/// Возвращает ближайшее число, которое делится на заданный делитель без остатка в большую сторону (по модулю).
		/// </summary>
		/// <typeparam name="T">Тип числа, реализующий интерфейс <see cref="INumber{T}"/>.</typeparam>
		/// <param name="value">Исходное число.</param>
		/// <param name="div">Делитель.</param>
		/// <returns>Ближайшее число, кратное <paramref name="div"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetNextDivisible<T>(
			T value,
			T div)
			where T : INumber<T>
		{
			if (div == T.Zero)
				return value;
			T rem1 = value % div;
			if (rem1 == T.Zero)
				return value;
			if (value < T.Zero)
				return value - rem1;
			return value + (div - rem1);
		}


		/// <summary>
		/// Возвращает индекс ближайшей точки, после которой находится значение, используя оптимизированный бинарный поиск.
		/// </summary>
		/// <typeparam name="T">Тип данных, реализующий интерфейс <see cref="IComparable{T}"/>.</typeparam>
		/// <param name="value">Проверяемое значение.</param>
		/// <param name="points">Набор упорядоченных контрольных точек (границ диапазонов).</param>
		/// <returns>
		/// Индекс зоны (начиная с 0). Если значение находится до первой точки — возвращает -1. 
		/// Если выходит за рамки последней точки — возвращает индекс последней зоны.
		/// </returns>
		public static int GetRangeIndex<T>(
			T value,
			params ReadOnlySpan<T> points)
			where T : IComparable<T>
		{
			if (points.Length < 2 || value.CompareTo(points[0]) < 0)
				return -1;
			int index = points.BinarySearch(value);
			if (index >= 0)
				return index == points.Length - 1
					? index - 1 : index;
			int bitwiseComplement = ~index;
			return bitwiseComplement >= points.Length
				? points.Length - 2 : bitwiseComplement - 1;
		}

	}

}
