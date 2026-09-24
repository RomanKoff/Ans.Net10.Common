// rev 2026-09-16

using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	public enum LetterCasesEnum
	{
		/// <summary>
		/// Без изменений
		/// </summary>
		Original,

		/// <summary>
		/// Нижний регистр (строчные)
		/// </summary>
		Lower,

		/// <summary>
		/// Верхний регистр (ПРОПИСНЫЕ, ЗАГЛАВНЫЕ)
		/// </summary>
		Upper,

		/// <summary>
		/// Первая буква строки заглавная
		/// </summary>
		FirstUpper,

		/// <summary>
		/// Заглавная Первая Буква В Каждом Слове Строки
		/// </summary>
		TitleCase
	}



	public static class SuppString
	{

		//	https://ru.stackoverflow.com/questions/1387144
		//	Support win1251 and koi8r:
		//		Using System.Text.Encoding.CodePages
		//		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		//	Program.cs ->
		//		SuppIO.Register_CodePagesEncodingProvider();


		/* functions */


		/// <summary>
		/// Преобразует строковые данные из кодировки Windows-1251 в UTF-8.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Перекодированная строка.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Convert_WINDOWS1251_UTF8(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_WINDOWS1251.GetBytes(source));
		}


		/// <summary>
		/// Преобразует строковые данные из кодировки KOI8-R в UTF-8.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Перекодированная строка.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Convert_KOI8R_UTF8(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_KOI8R.GetBytes(source));
		}


		/// <summary>
		/// Преобразует строковые данные из кодировки CP866 в UTF-8.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Перекодированная строка.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Convert_CP866_UTF8(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_CP866.GetBytes(source));
		}


		/// <summary>
		/// Преобразует строковые данные из кодировки ISO-8859-1 в UTF-8.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Перекодированная строка.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Convert_ISO88591_UTF8(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_ISO88591.GetBytes(source));
		}


		/// <summary>
		/// Проверяет, является ли хотя бы одна из переданных строк непустой. 
		/// Использует params ReadOnlySpan в .NET 10 для исключения аллокаций памяти.
		/// </summary>
		/// <param name="values">Набор проверяемых строк.</param>
		/// <returns><see langword="true"/>, если найдена хотя бы одна непустая строка; иначе — <see langword="false"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(
			params ReadOnlySpan<string?> values)
		{
			foreach (var value1 in values)
				if (!string.IsNullOrEmpty(value1))
					return true;
			return false;
		}


		/// <summary>
		/// Проверяет, что все переданные строки являются непустыми. 
		/// Использует params ReadOnlySpan в .NET 10 для исключения аллокаций памяти.
		/// </summary>
		/// <param name="values">Набор проверяемых строк.</param>
		/// <returns><see langword="true"/>, если все строки непустые; иначе — <see langword="false"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAll(
			params ReadOnlySpan<string?> values)
		{
			foreach (var value1 in values)
				if (string.IsNullOrEmpty(value1))
					return false;
			return true;
		}


		/// <summary>
		/// Объединяет непустые элементы в одну строку, опционально форматируя каждый элемент
		/// и оборачивая итоговый результат по шаблону.
		/// </summary>
		/// <param name="resultTemplate">Шаблон для итоговой сборки (например, "Результат: {0}"). Если пуст, обертка не применяется.</param>
		/// <param name="itemTemplate">Шаблон форматирования для каждого отдельного элемента (например, "[{0}]").</param>
		/// <param name="separator">Строка-разделитель между элементами.</param>
		/// <param name="items">Набор объединяемых строк.</param>
		/// <returns>Итоговая отформатированная строка. Если на выходе получилась пустая строка, возвращает <see cref="string.Empty"/>.</returns>
		public static string Join(
			string resultTemplate,
			string itemTemplate,
			string separator,
			params ReadOnlySpan<string?> items)
		{
			var sb1 = new StringBuilder();
			bool f1 = true;
			bool hasTemplateItem1 = !string.IsNullOrEmpty(itemTemplate);
			foreach (var s1 in items)
				if (!string.IsNullOrEmpty(s1))
				{
					if (f1)
						f1 = false;
					else
						sb1.Append(separator);
					sb1.Append(hasTemplateItem1 ? string.Format(itemTemplate, s1) : s1);
				}
			var s2 = sb1.ToString();
			if (string.IsNullOrEmpty(s2))
				return string.Empty;
			return string.IsNullOrEmpty(resultTemplate)
				? s2 : string.Format(resultTemplate, s2);
		}


		/// <summary>
		/// Разделяет строку вида "ключ=значение" на пару ключ и значение. 
		/// Если разделитель отсутствует, возвращает исходную строку в качестве ключа и значения.
		/// </summary>
		/// <param name="definition">Исходная строка для разделения.</param>
		/// <returns>Объект <see cref="KeyValuePair{TKey, TValue}"/> с результатами разделения.</returns>
		public static KeyValuePair<string, string> GetPair(
			string definition)
		{
			if (string.IsNullOrEmpty(definition))
				return new KeyValuePair<string, string>(string.Empty, string.Empty);
			int i1 = definition.IndexOf('=');
			if (i1 >= 0)
				return new KeyValuePair<string, string>(
					definition[..i1], definition[(i1 + 1)..]);
			return new KeyValuePair<string, string>(definition, definition);
		}


		/// <summary>
		/// Преобразует регистр строки на основе переданного режима <see cref="LetterCasesEnum"/>.
		/// </summary>
		/// <param name="value">Исходная строка.</param>
		/// <param name="textCase">Целевой регистр символов.</param>
		/// <param name="forcedToLower">Принудительно переводить все остальные буквы строки в нижний регистр (актуально для FirstUpper и TitleCase).</param>
		/// <returns>Преобразованная строка. Если входная строка пуста или null, возвращает <see cref="string.Empty"/>.</returns>
		public static string GetModCase(
			string value,
			LetterCasesEnum textCase,
			bool forcedToLower = false)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			return textCase switch
			{
				LetterCasesEnum.Lower => value.ToLowerInvariant(),
				LetterCasesEnum.Upper => value.ToUpperInvariant(),
				LetterCasesEnum.FirstUpper => value.GetAsFirstUpper(forcedToLower),
				LetterCasesEnum.TitleCase => value.GetAsTitleCase(forcedToLower),
				_ => value
			};
		}


		/// <summary>
		/// Заменяет все управляющие спецсимволы (с ASCII-кодом меньше 32) на обычные пробелы.
		/// </summary>
		/// <param name="value">Исходная строка.</param>
		/// <returns>Строка с нормализованными символами. Если на входе пустая строка, возвращает <see cref="string.Empty"/>.</returns>
		public static string GetFixSpecChars(
			string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			var sb1 = new StringBuilder(value.Length);
			foreach (var char1 in value)
				sb1.Append(char1 < 32 ? ' ' : char1);
			return sb1.ToString();
		}


		/// <summary>
		/// Возвращает безопасную алфавитно-цифровую строку в нижнем регистре (латиница и цифры).
		/// Предварительно транслитерирует кириллицу, а любые спецсимволы заменяет дефисами.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Безопасная алфавитно-цифровая строка. Если входная строка пуста или null, возвращает пустую строку.</returns>
		public static string GetSafeNumberString(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			var s1 = SuppLangRu.GetTranslitRuToEn(source.ToLowerInvariant());
			return s1
				.ReplaceByChars("-", "^0-9a-z")
				.GetReplaceRecursively("--", "-")
				.Trim('-');
		}


		/// <summary>
		/// Возвращает безопасную строку-идентификатор. Заменяет все пробелы и недопустимые символы на подчеркивания.
		/// Символы с ASCII-кодом больше 126 кодируются в формат hex-вида (x[код]). Ёмкость буфера рассчитывается без аллокаций.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Безопасная строка идентификатора. Если входная строка пуста или null, возвращает <see cref="string.Empty"/>.</returns>
		public static string GetSafeNameString(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			var s1 = SuppLangRu.GetTranslitRuToEn(source.ToLowerInvariant());
			var span1 = s1.AsSpan();
			var sb1 = _prepareSafe(span1);
			foreach (char ch1 in span1)
				switch (ch1)
				{
					case >= '0' and <= '9':
					case >= 'a' and <= 'z':
						sb1.Append(ch1);
						break;
					case > (char)126:
						sb1.Append('x').Append((int)ch1);
						break;
					default:
						sb1.Append('_');
						break;
				}
			return sb1.ToString().GetReplaceRecursively("__", "_");
		}


		/// <summary>
		/// Возвращает полностью безопасную строку для использования в файловой системе (в именах файлов или папок).
		/// Очищает от запрещенных спецсимволов, сохраняя знаки дефиса, подчеркивания, тильды и каретки.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Безопасная строка для путей файловой системы. Если входная строка пуста или null, возвращает <see cref="string.Empty"/>.</returns>
		public static string GetSafeFsString(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			var s1 = SuppLangRu.GetTranslitRuToEn(source.ToLowerInvariant());
			var span1 = s1.AsSpan();
			var sb1 = _prepareSafe(span1);
			foreach (char ch1 in span1)
				switch (ch1)
				{
					case >= '0' and <= '9':
					case >= 'a' and <= 'z':
					case '-':
					case '^':
					case '_':
					case '~':
						sb1.Append(ch1);
						break;
					case > (char)126:
						sb1.Append('x').Append((int)ch1);
						break;
					default:
						sb1.Append('_');
						break;
				}
			return sb1.ToString()
				.GetReplaceRecursively("__", "_")
				.GetReplaceRecursively("_-", "-")
				.GetReplaceRecursively("-_", "-")
				.GetReplaceRecursively("--", "-");
		}


		/* privates */


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static StringBuilder _prepareSafe(
			ReadOnlySpan<char> span)
		{
			int finalCapacity1 = 0;
			foreach (char ch1 in span)
			{
				if (ch1 > (char)126)
				{
					int code = (int)ch1;
					int digitCount = code switch
					{
						< 1000 => 3,
						< 10000 => 4,
						_ => 5
					};
					finalCapacity1 += 1 + digitCount;
				}
				else
					finalCapacity1 += 1;
			}
			return new StringBuilder(finalCapacity1);
		}

	}

}
