// rev 2026-09-16

using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Ans.Net10.Common
{

	public static partial class _e_string
	{

		/// <summary>
		/// Производит замену <paramref name="oldValue"/> на <paramref name="newValue"/>
		/// рекурсивно до тех пор, пока заменяемая подстрока полностью не исчезнет из текста.
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="oldValue">Подстрока, которую необходимо заменить.</param>
		/// <param name="newValue">Подстрока, на которую производится замена.</param>
		/// <returns>Результирующая строка после всех рекурсивных замен.</returns>
		public static string GetReplaceRecursively(
			this string? instance,
			string oldValue,
			string newValue)
		{
			if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(oldValue))
				return instance ?? string.Empty;
			if (newValue != null && newValue.Contains(oldValue))
				return instance.Replace(oldValue, newValue);
			var sb1 = new StringBuilder(instance);
			int len1;
			do
			{
				len1 = sb1.Length;
				sb1.Replace(oldValue, newValue);
			}
			while (sb1.Length != len1);
			return sb1.ToString();
		}


		/// <summary>
		/// Производит замену подстрок в строке на основе переданного словаря соответствий.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <param name="dict">Словарь, где ключ — заменяемое значение, а значение — новая подстрока.</param>
		/// <returns>Строка с выполненными заменами.</returns>
		public static string ReplaceFromDict(
			this string source,
			Dictionary<string, string> dict)
		{
			if (string.IsNullOrEmpty(source) || dict == null || dict.Count == 0)
				return source;
			var sb1 = new StringBuilder(source);
			foreach (var item1 in dict)
				if (!string.IsNullOrEmpty(item1.Key))
					sb1.Replace(item1.Key, item1.Value);
			return sb1.ToString();
		}


		/// <summary>
		/// Возвращает новую строку, если исходная строка полностью эквивалентна сравниваемой.
		/// В противном случае возвращает исходную строку.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <param name="compared">Строка для сравнения.</param>
		/// <param name="newest">Строка, на которую нужно заменить в случае равенства.</param>
		/// <returns>Результат условной замены строки.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ReplaceIfEqual(
			this string source,
			string compared,
			string newest)
		{
			return source == compared
				? newest : source;
		}


		/// <summary>
		/// Заменяет любые символы из указанного паттерна заданной строкой-маской.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <param name="mask">Строка-маска для замены найденных символов.</param>
		/// <param name="charsPattern">Строка, содержащая набор заменяемых символов.</param>
		/// <returns>Строка, в которой целевые символы заменены маской.</returns>
		public static string ReplaceByChars(
			this string source,
			string mask,
			string charsPattern)
		{
			if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(charsPattern))
				return source;
			var sb1 = new StringBuilder(source.Length);
			var span1 = charsPattern.AsSpan();
			foreach (var c1 in source)
				if (span1.Contains(c1))
					sb1.Append(mask);
				else
					sb1.Append(c1);
			return sb1.ToString();
		}


		/// <summary>
		/// Заменяет любые символы из указанного массива заданной строкой-маской. Zero-allocation перегрузка.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <param name="mask">Строка-маска для замены найденных символов.</param>
		/// <param name="chars">Массив заменяемых символов.</param>
		/// <returns>Строка, в которой целевые символы заменены маской.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ReplaceByChars(
			this string source,
			string mask,
			params char[] chars)
		{
			if (string.IsNullOrEmpty(source) || chars == null || chars.Length == 0)
				return source;
			var sb1 = new StringBuilder(source.Length);
			var span1 = chars.AsSpan();
			foreach (var c1 in source)
				if (span1.Contains(c1))
					sb1.Append(mask);
				else
					sb1.Append(c1);
			return sb1.ToString();
		}


		/// <summary>
		/// Производит замену хэштегов в строке на основе переданного словаря.
		/// </summary>
		/// <param name="source">Исходная строка, содержащая хэштеги.</param>
		/// <param name="dictionary">Словарь соответствия имен хэштегов (без маркера) и их замен.</param>
		/// <param name="marker">Символ-маркер хэштега (по умолчанию '#').</param>
		/// <returns>Строка с замененными хэштегами.</returns>
		public static string ReplaceHashtagsFromDict(
			this string source,
			Dictionary<string, string> dictionary,
			char marker = '#')
		{
			if (string.IsNullOrEmpty(source) || dictionary == null || dictionary.Count == 0)
				return source;
			string marker1 = Regex.Escape(marker.ToString());
			return Regex.Replace(source, $"{marker1}([\\w\\d]+)", x =>
			{
				var hash1 = x.Groups[1].Value;
				return dictionary.TryGetValue(hash1, out var replacement1)
					? replacement1 : x.Value;
			});
		}


		/// <summary>
		/// Находит непрерывно повторяющуюся строку-маску в НАЧАЛЕ строки и заменяет все ее вхождения указанной подстрокой.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <param name="mask">Повторяющаяся подстрока-маска, которую нужно найти.</param>
		/// <param name="replacement">Строка, на которую заменяется каждое вхождение маски (опционально).</param>
		/// <returns>Результирующая строка с произведенной заменой в начале.</returns>
		public static string ReplaceStart(
			this string source,
			string mask,
			string? replacement = null)
		{
			if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(mask))
				return source;
			int sourceLen1 = source.Length;
			int maskLen1 = mask.Length;
			int curr1 = 0;
			var sourceSpan1 = source.AsSpan();
			var maskSpan1 = mask.AsSpan();
			while (curr1 + maskLen1 <= sourceLen1
				&& sourceSpan1.Slice(curr1, maskLen1).SequenceEqual(maskSpan1))
				curr1 += maskLen1;
			if (curr1 == 0)
				return source;
			int repeats1 = curr1 / maskLen1;
			var sb1 = new StringBuilder(sourceLen1 + (repeats1 * (replacement?.Length ?? 0)));
			if (!string.IsNullOrEmpty(replacement))
				for (int i1 = 0; i1 < repeats1; i1++)
					sb1.Append(replacement);
			sb1.Append(sourceSpan1[curr1..]);
			return sb1.ToString();
		}


		/// <summary>
		/// Находит непрерывно повторяющуюся строку-маску в КОНЦЕ строки и заменяет все ее вхождения указанной подстрокой.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <param name="mask">Повторяющаяся подстрока-маска, которую нужно найти.</param>
		/// <param name="replacement">Строка, на которую заменяется каждое вхождение маски (опционально).</param>
		/// <returns>Результирующая строка с произведенной заменой в конце.</returns>
		public static string ReplaceEnd(
			this string source,
			string mask,
			string? replacement = null)
		{
			if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(mask))
				return source;
			int sourceLen1 = source.Length;
			int maskLen1 = mask.Length;
			int curr1 = sourceLen1;
			var sourceSpan1 = source.AsSpan();
			var maskSpan1 = mask.AsSpan();
			while (curr1 - maskLen1 >= 0
				&& sourceSpan1.Slice(curr1 - maskLen1, maskLen1).SequenceEqual(maskSpan1))
				curr1 -= maskLen1;
			if (curr1 == sourceLen1)
				return source;
			int repeats1 = (sourceLen1 - curr1) / maskLen1;
			var sb1 = new StringBuilder(curr1 + (repeats1 * (replacement?.Length ?? 0)));
			sb1.Append(sourceSpan1[..curr1]);
			if (!string.IsNullOrEmpty(replacement))
				for (int i1 = 0; i1 < repeats1; i1++)
					sb1.Append(replacement);
			return sb1.ToString();
		}

	}

}
