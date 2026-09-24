// rev 2026-09-02

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	public static partial class Exts__make
	{

		/// <summary>
		/// Форматирует строку по шаблону. Возвращает пустую строку, если значение пустое.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Make(
			this string? value,
			string? template)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			if (string.IsNullOrEmpty(template))
				return value;
			return string.Format(template, value);
		}


		/// <summary>
		/// Форматирует строку по шаблону. Возвращает пустую строку, если значение совпадает с <paramref name="nullValue"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Make(
			this string? value,
			string? template,
			string? nullValue)
		{
			if (string.Equals(value, nullValue))
				return string.Empty;
			if (string.IsNullOrEmpty(template))
				return value ?? string.Empty;
			return string.Format(template, value);
		}


		/// <summary>
		/// Форматирует строку по шаблону с поддержкой дополнительных аргументов.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string MakeMore(
			this string? value,
			string? template,
			params object?[] args)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			if (string.IsNullOrEmpty(template))
				return value;
			object?[] args1 = [value, .. args];
			return string.Format(template, args1);
		}


		/// <summary>
		/// Форматирует структуру с шаблоном, возвращая <see cref="string.Empty"/>,
		/// если значение совпадает с <paramref name="nullValue"/>.
		/// </summary>
		public static string Make<T>(
			this T value,
			string? template,
			string? format,
			T nullValue,
			IFormatProvider? provider = null)
			where T : struct, IFormattable
		{
			if (value.Equals(nullValue))
				return string.Empty;
			provider ??= CultureInfo.InvariantCulture;
			string s1 = string.IsNullOrEmpty(format)
				? value.ToString(null, provider)
				: value.ToString(format, provider);
			return s1.Make(template);
		}


		/// <summary>
		/// Форматирует структуру с автоматическим определением дефолтных пустых значений (MinValue для дат, 0 для чисел).
		/// </summary>
		public static string Make<T>(
			this T value,
			string? template,
			string? format = null,
			IFormatProvider? provider = null)
			where T : struct, IFormattable
		{
			if (value is DateTime dateTime && dateTime == DateTime.MinValue
				|| value is DateOnly dateOnly && dateOnly == DateOnly.MinValue
				|| value is TimeOnly timeOnly && timeOnly == TimeOnly.MinValue
				|| value.Equals(default(T)))
				return string.Empty;
			return value.Make(template, format, default, provider);
		}


		/// <summary>
		/// Форматирует nullable-структуру. Если значения нет, сразу возвращает <see cref="string.Empty"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Make<T>(
			this T? value,
			string? template,
			string? format = null,
			IFormatProvider? provider = null)
			where T : struct, IFormattable
		{
			return value.HasValue
				? value.Value.Make(template, format, provider)
				: string.Empty;
		}

		/// <summary>
		/// Форматирует значение перечисления по шаблону. Если задан атрибут [Description],
		/// берет его текст, иначе — имя элемента.
		/// </summary>
		public static string Make<T>(
			this T value,
			string? template)
			where T : struct, Enum
		{
			string? name1 = Enum.GetName(value);
			if (string.IsNullOrEmpty(name1))
				return string.Empty;
			var field1 = typeof(T).GetField(name1);
			if (field1?.GetCustomAttribute<DescriptionAttribute>() is { Description: { Length: > 0 } desc1 })
				name1 = desc1;
			return name1.Make(template);
		}


		/// <summary>
		/// Форматирует значение перечисления по шаблону. Если значение совпадает с <paramref name="nullValue"/>,
		/// возвращает <see cref="string.Empty"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Make<T>(
			this T value,
			string? template,
			T nullValue)
			where T : struct, Enum
		{
			return value.Equals(nullValue)
				? string.Empty
				: value.Make(template);
		}


		/// <summary>
		/// Возвращает <paramref name="trueText"/>, если логическое значение истинно,
		/// иначе возвращает <paramref name="falseText"/> или пустую строку.
		/// </summary>
		/// <param name="falseText">
		/// <para><i>Допускает null. Значение по умолчанию:</i> <see langword="null"/>.</para>
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Make(
			this bool value,
			string trueText,
			string? falseText = null)
		{
			return value
				? trueText
				: (falseText ?? string.Empty);
		}


		/// <summary>
		/// Форматирует строку по шаблону, если логическое значение истинно.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string MakeMore(
			this bool value,
			string template,
			params object?[] args)
		{
			return value
				? string.Format(template, args)
				: string.Empty;
		}


		/// <summary>
		/// Повторяет строку заданное количество раз, форматируя элементы и объединяя их через разделитель.
		/// </summary>
		public static string MakeRepeats(
			this string? value,
			int count,
			string? resultTemplate = null,
			string? itemTemplate = null,
			string? itemsSeparator = null)
		{
			if (string.IsNullOrEmpty(value) || count < 1)
				return string.Empty;
			var item1 = value.Make(itemTemplate);
			if (count == 1)
				return item1.Make(resultTemplate);
			int separatorLength1 = itemsSeparator?.Length ?? 0;
			int estimatedCapacity1 = (item1.Length * count) + (separatorLength1 * (count - 1));
			var sb1 = new StringBuilder(estimatedCapacity1);
			for (int i1 = 0; i1 < count; i1++)
			{
				sb1.Append(item1);
				if (i1 < count - 1 && separatorLength1 > 0)
					sb1.Append(itemsSeparator);
			}
			return sb1.ToString().Make(resultTemplate);
		}


		/// <summary>
		/// Собирает строку из коллекции строк, пропуская пустые элементы, форматируя их и объединяя через разделитель.
		/// </summary>
		public static string MakeFromCollection(
			this IEnumerable<string?>? items,
			string? resultTemplate = null,
			string? itemTemplate = null,
			string? itemsSeparator = null)
		{
			if (items == null)
				return string.Empty;
			var sb1 = new StringBuilder();
			bool isFirst1 = true;
			foreach (var item1 in items)
			{
				if (string.IsNullOrEmpty(item1))
					continue;
				var formattedItem1 = string.IsNullOrEmpty(itemTemplate)
					? item1 : item1.Make(itemTemplate);
				if (string.IsNullOrEmpty(formattedItem1))
					continue;
				if (!isFirst1 && !string.IsNullOrEmpty(itemsSeparator))
					sb1.Append(itemsSeparator);
				sb1.Append(formattedItem1);
				isFirst1 = false;
			}
			var result1 = sb1.ToString();
			return string.IsNullOrEmpty(result1)
				? string.Empty
				: result1.Make(resultTemplate);
		}


		/// <summary>
		/// Собирает строку из коллекции любых объектов, извлекая текстовое значение с помощью экстрактора.
		/// </summary>
		public static string MakeFromCollection<T>(
			this IEnumerable<T>? items,
			Func<T, string?> itemExtractor,
			string? resultTemplate = null,
			string? itemTemplate = null,
			string? itemsSeparator = null)
		{
			if (items == null || itemExtractor == null)
				return string.Empty;
			var sb1 = new StringBuilder();
			bool isFirst1 = true;
			foreach (var item1 in items)
			{
				if (item1 is null)
					continue;
				string? extracted = itemExtractor(item1);
				if (string.IsNullOrEmpty(extracted))
					continue;
				string formattedItem1 = string.IsNullOrEmpty(itemTemplate)
					? extracted : extracted.Make(itemTemplate);
				if (string.IsNullOrEmpty(formattedItem1))
					continue;
				if (!isFirst1 && !string.IsNullOrEmpty(itemsSeparator))
					sb1.Append(itemsSeparator);
				sb1.Append(formattedItem1);
				isFirst1 = false;
			}
			var result1 = sb1.ToString();
			return string.IsNullOrEmpty(result1)
				? string.Empty
				: result1.Make(resultTemplate);
		}


		/// <summary>
		/// Находит значение по ключу в словаре, преобразует его через функцию-экстрактор
		/// и форматирует по шаблону. Если ключ не найден, форматирует сам ключ.
		/// </summary>
		public static string MakeOverDict<T>(
			this string? key,
			IDictionary<string, T>? dictionary,
			Func<T, string?> itemExtractor,
			string? template)
		{
			if (string.IsNullOrEmpty(key) || dictionary == null || itemExtractor == null)
				return string.Empty;
			string? s1 = dictionary.TryGetValue(key, out var value)
				? itemExtractor(value) : key;
			return (s1 ?? string.Empty).Make(template);
		}


		/// <summary>
		/// Находит строковое значение по ключу в словаре и форматирует его по шаблону.
		/// Если ключ не найден, форматирует сам ключ.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string MakeOverDict(
			this string? key,
			IDictionary<string, string?>? dictionary,
			string? template)
		{
			if (string.IsNullOrEmpty(key) || dictionary == null)
				return string.Empty;
			string? s1 = dictionary.TryGetValue(key, out var value)
				? value : key;
			return (s1 ?? string.Empty).Make(template);
		}


		/// <summary>
		/// Форматирует URL по шаблону. Автоматически добавляет префикс 'https://', если он отсутствует.
		/// </summary>
		public static string Make_UrlWrapper(
			this string? url,
			string? template)
		{
			if (string.IsNullOrEmpty(url))
				return string.Empty;
			if (url.StartsWith('/')
				|| url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
				|| url.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
				return url.Make(template);
			return $"https://{url}".Make(template);
		}

	}

}
