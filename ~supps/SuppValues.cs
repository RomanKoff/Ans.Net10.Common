// rev 2026-09-21

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Определяет варианты биологического пола человека.
	/// </summary>
	public enum GenderEnum : int
	{
		/// <summary>
		/// Не указан.
		/// </summary>
		NotSpecified = 0,

		/// <summary>
		/// Мужской.
		/// </summary>
		Male = 1,

		/// <summary>
		/// Женский.
		/// </summary>
		Female = 2
	}



	/// <summary>
	/// Вспомогательный класс для работы
	/// со значениями входящих переменных.
	/// </summary>
	public static class SuppValues
	{

		/// <summary>
		/// Возвращает исходную строку, если она не пустая; в противном случае возвращает
		/// первое непустое значение из списка альтернатив.
		/// </summary>
		/// <param name="current">Проверяемая строка.</param>
		/// <param name="defaultValues">
		/// Набор альтернативных значений по умолчанию (передается без аллокаций в куче через ReadOnlySpan).
		/// </param>
		/// <returns>Первая непустая строка или <see langword="null"/>, если все значения пусты.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Default(
			string current,
			params ReadOnlySpan<string?> defaultValues)
		{
			if (!string.IsNullOrEmpty(current))
				return current;
			foreach (var value1 in defaultValues)
				if (!string.IsNullOrEmpty(value1))
					return value1;
			return null!;
		}


		/// <summary>
		/// Возвращает значение по умолчанию, если текущее число совпадает со значением, интерпретируемым как null.
		/// </summary>
		/// <param name="current">Текущее числовое значение.</param>
		/// <param name="defaultValue">Значение по умолчанию.</param>
		/// <param name="nullValue">Значение, которое считается эквивалентом отсутствия данных (по умолчанию 0).</param>
		/// <returns>Исходное число или альтернативное значение по умолчанию.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Default(
			int current,
			int defaultValue,
			int nullValue = 0)
		{
			return (current == nullValue)
				? defaultValue : current;
		}


		/// <summary>
		/// Проверяет, является ли хотя бы один из переданных объектов непустым
		/// (не null и не пустой строкой) без лишних строковых аллокаций.
		/// </summary>
		/// <param name="values">Набор проверяемых объектов произвольного типа.</param>
		/// <returns>
		/// <see langword="true"/>, если найден хотя бы один заполненный объект; иначе — <see langword="false"/>.
		/// </returns>
		public static bool HasAny(
			params ReadOnlySpan<object?> values)
		{
			foreach (var value1 in values)
			{
				if (value1 == null)
					continue;
				if (value1 is string str1)
				{
					if (!string.IsNullOrEmpty(str1))
						return true;
				}
				else if (value1 is IFormattable
					|| value1.GetType().IsPrimitive)
					return true;
				else if (!string.IsNullOrEmpty(value1.ToString()))
					return true;
			}
			return false;
		}


		/// <summary>
		/// Проверяет, что все переданные объекты являются непустыми (не null и не содержат пустых строк).
		/// </summary>
		/// <param name="values">Набор проверяемых объектов произвольного типа.</param>
		/// <returns>
		/// <see langword="true"/>, если все объекты заполнены; иначе — <see langword="false"/>.
		/// </returns>
		public static bool HasAll(
			params ReadOnlySpan<object> values)
		{
			foreach (var value1 in values)
			{
				if (value1 == null)
					return false;
				if (value1 is string str1)
				{
					if (string.IsNullOrEmpty(str1))
						return false;
				}
				else if (value1 is IFormattable
					|| value1.GetType().IsPrimitive)
					continue;
				else if (string.IsNullOrEmpty(value1.ToString()))
					return false;
			}
			return true;
		}


		/// <summary>
		/// Возвращает максимальное значение из двух.
		/// Если второе значение не задано, возвращает первое.
		/// </summary>
		/// <typeparam name="T">Тип структуры, поддерживающий операторы сравнения.</typeparam>
		/// <param name="value1">Первое сравниваемое значение.</param>
		/// <param name="value2">Второе сравниваемое значение (может быть <see langword="null"/>).</param>
		/// <returns>
		/// Наибольшее из двух значений, либо <paramref name="value1"/>,
		/// если <paramref name="value2"/> равно <see langword="null"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MaxValue<T>(
			T value1,
			T? value2)
			where T : struct, IComparable<T>
		{
			if (!value2.HasValue)
				return value1;
			T v2 = value2.Value;
			return value1.CompareTo(v2) > 0
				? value1 : v2;
		}


		/// <summary>
		/// Возвращает минимальное значение из двух.
		/// Если второе значение не задано, возвращает первое.
		/// </summary>
		/// <typeparam name="T">Тип структуры, поддерживающий операторы сравнения.</typeparam>
		/// <param name="value1">Первое сравниваемое значение.</param>
		/// <param name="value2">Второе сравниваемое значение (может быть <see langword="null"/>).</param>
		/// <returns>
		/// Наименьшее из двух значений, либо <paramref name="value1"/>,
		/// если <paramref name="value2"/> равно <see langword="null"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MinValue<T>(
			T value1,
			T? value2)
			where T : struct, IComparable<T>
		{
			if (!value2.HasValue)
				return value1;
			T v2 = value2.Value;
			return value1.CompareTo(v2) < 0
				? value1 : v2;
		}


		/// <summary>
		/// Возвращает максимальное числовое значение из двух.
		/// Если второе число не задано, возвращает первое.
		/// </summary>
		/// <typeparam name="T">Тип числа, реализующий интерфейс <see cref="INumber{T}"/>.</typeparam>
		/// <param name="value1">Первое сравниваемое число.</param>
		/// <param name="value2">Второе сравниваемое число (может быть <see langword="null"/>).</param>
		/// <returns>
		/// Наибольшее из двух чисел, либо <paramref name="value1"/>,
		/// если <paramref name="value2"/> равно <see langword="null"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MaxNum<T>(
			T value1,
			T? value2)
			where T : struct, INumber<T>
		{
			return value2.HasValue
				? T.Max(value1, value2.Value)
				: value1;
		}


		/// <summary>
		/// Возвращает минимальное числовое значение из двух.
		/// Если второе число не задано, возвращает первое.
		/// </summary>
		/// <typeparam name="T">Тип числа, реализующий интерфейс <see cref="INumber{T}"/>.</typeparam>
		/// <param name="value1">Первое сравниваемое число.</param>
		/// <param name="value2">Второе сравниваемое число (может быть <see langword="null"/>).</param>
		/// <returns>
		/// Наименьшее из двух чисел, либо <paramref name="value1"/>,
		/// если <paramref name="value2"/> равно <see langword="null"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T MinNum<T>(
			T value1,
			T? value2)
			where T : struct, INumber<T>
		{
			return value2.HasValue
				? T.Min(value1, value2.Value)
				: value1;
		}


		/// <summary>
		/// Преобразует базовые типы данных (.NET структуры дат, времени и логики)
		/// в их строковые веб-эквиваленты.
		/// </summary>
		/// <param name="value">Объект для сериализации.</param>
		/// <returns>Строковое веб-представление объекта.</returns>
		public static string GetStringForWeb(
			object value)
		{
			if (value == null)
				return string.Empty;
			return value switch
			{
				DateTime dt1 => dt1.ToString("u"),
				DateOnly do1 => do1.ToString("yyyy-MM-dd"),
				TimeOnly to1 => to1.ToString("HH\\:mm\\:ss.fff"),
				bool b1 => b1.Make("true", "false"),
				_ => value.ToString() ?? string.Empty
			};
		}


		/// <summary>
		/// Извлекает из строки исключительно цифровые символы
		/// на основе регулярного выражения фильтрации.
		/// </summary>
		/// <param name="number">Входящая алфавитно-цифровая строка.</param>
		/// <returns>Строка, состоящая только из цифр.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetDigitalOnly(
			string number)
		{
			if (string.IsNullOrEmpty(number))
				return string.Empty;
			return _Consts.G_REGEX_NOT_NUMBER().Replace(number, "");
		}


		/// <summary>
		/// Форматирует вещественное число в локализованную денежную строку
		/// с двумя знаками после запятой.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetCurrencyLoc(
			float amount)
		{
			return string.Format("{0:N2}", amount);
		}


		/// <summary>
		/// Форматирует число двойной точности в локализованную денежную строку
		/// с двумя знаками после запятой.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetCurrencyLoc(
			double amount)
		{
			return string.Format("{0:N2}", amount);
		}


		/// <summary>
		/// Форматирует высокоточное десятичное число в локализованную денежную строку
		/// с двумя знаками после запятой.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetCurrencyLoc(
			decimal amount)
		{
			return string.Format("{0:N2}", amount);
		}


		/// <summary>
		/// Форматирует вещественное число в бухгалтерский вид разделителя рублей
		/// и копеек через знак равенства (Рубли=Копейки).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetCurrencyBuh(
			float amount)
		{
			long rub1 = (long)amount;
			long kop1 = Math.Abs((long)Math.Round(amount * 100)) % 100;
			return string.Format("{0}={1:00}", rub1, kop1);
		}


		/// <summary>
		/// Форматирует число двойной точности в бухгалтерский вид разделителя рублей
		/// и копеек через знак равенства (Рубли=Копейки).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetCurrencyBuh(
			double amount)
		{
			long rub1 = (long)amount;
			long kop1 = Math.Abs((long)Math.Round(amount * 100)) % 100;
			return string.Format("{0}={1:00}", rub1, kop1);
		}


		/// <summary>
		/// Форматирует высокоточное десятичное число в бухгалтерский вид разделителя рублей
		/// и копеек через знак равенства (Рубли=Копейки).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetCurrencyBuh(
			decimal amount)
		{
			long rub1 = (long)amount;
			long kop1 = Math.Abs((long)Math.Round(amount * 100)) % 100;
			return string.Format("{0}={1:00}", rub1, kop1);
		}

	}

}
