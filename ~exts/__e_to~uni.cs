// rev 2026-09-02

using System.Globalization;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class __e_to
	{

		// --- Целочисленные типы (int, uint, long)


		/// <summary>
		/// Преобразует строку в nullable int.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int? ToInt(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<int>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в int. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ToInt(
			this string? value,
			int defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в nullable uint.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint? ToUInt(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<uint>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в uint. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint ToUInt(
			this string? value,
			uint defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в nullable long.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long? ToLong(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<long>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в long. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long ToLong(
			this string? value,
			long defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		// --- Типы с плавающей запятой и фиксированной точностью (double, float, decimal)


		/// <summary>
		/// Преобразует строку в nullable double.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double? ToDouble(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<double>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в double. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double ToDouble(
			this string? value,
			double defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в nullable float.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float? ToFloat(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<float>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в float. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ToFloat(
			this string? value,
			float defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в nullable decimal.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal? ToDecimal(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<decimal>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в decimal. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static decimal ToDecimal(
			this string? value,
			decimal defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		// --- Дата и время (DateTime, DateOnly, TimeOnly)


		/// <summary>
		/// Преобразует строку в nullable DateTime с использованием InvariantCulture.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateTime? ToDateTime(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<DateTime>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в DateTime. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateTime ToDateTime(
			this string? value,
			DateTime defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в nullable DateOnly с использованием InvariantCulture.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateOnly? ToDateOnly(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<DateOnly>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в DateOnly. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateOnly ToDateOnly(
			this string? value,
			DateOnly defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в nullable TimeOnly с использованием InvariantCulture.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeOnly? ToTimeOnly(
			this string? value,
			IFormatProvider? provider = null)
			=> _parse<TimeOnly>(value, provider ?? CultureInfo.InvariantCulture);


		/// <summary>
		/// Преобразует строку в TimeOnly. Если преобразование невозможно,
		/// возвращает значение по умолчанию.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TimeOnly ToTimeOnly(
			this string? value,
			TimeOnly defaultValue,
			IFormatProvider? provider = null)
			=> _parse(value, defaultValue, provider ?? CultureInfo.InvariantCulture);



		// --- Логический тип (bool)


		/// <summary>
		/// Преобразует строку в логическое значение (bool). 
		/// Возвращает true, если строка равна "1", "+" или "true" (без учета регистра).
		/// В остальных случаях — false.
		/// </summary>
		/// <param name="value">Исходная строка для преобразования.</param>
		/// <returns>Значение типа bool.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ToBool(
			this string? value)
		{
			if (value == null)
				return false;
			var span1 = value.AsSpan().Trim();
			if (span1.IsEmpty)
				return false;
			if (span1.Equals("1", StringComparison.Ordinal))
				return true;
			if (span1.Equals("+", StringComparison.Ordinal))
				return true;
			return span1.Equals("true", StringComparison.OrdinalIgnoreCase);
		}


		/* privates */


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T? _parse<T>(
			string? value,
			IFormatProvider provider)
			where T : struct, ISpanParsable<T>
		{
			if (value == null)
				return null;
			var span1 = value.AsSpan().Trim();
			if (span1.IsEmpty)
				return null;
			return T.TryParse(span1, provider, out var result1)
				? result1 : null;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static T _parse<T>(
			string? value,
			T defaultValue,
			IFormatProvider provider)
			where T : struct, ISpanParsable<T>
		{
			return _parse<T>(value, provider) ?? defaultValue;
		}

	}

}
