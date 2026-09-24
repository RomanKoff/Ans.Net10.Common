// rev 2026-09-14

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class _e_string
	{

		/// <summary>
		/// Разделяет строку на фиксированное количество элементов.
		/// Недостающие элементы заполняются пустой строкой, лишние — отсекаются.
		/// </summary>
		/// <param name="instance">Исходная строка для разделения.</param>
		/// <param name="separator">Строка-разделитель.</param>
		/// <param name="count">Требуемое фиксированное количество элементов в возвращаемом массиве.</param>
		/// <returns>Массив строк фиксированной длины.</returns>
		public static string[] SplitFix(
			this string? instance,
			string separator,
			int count)
		{
			if (count <= 0)
				return [];
			if (string.IsNullOrEmpty(instance))
			{
				var result = new string[count];
				Array.Fill(result, string.Empty);
				return result;
			}
			var a0 = instance.Split(separator, count + 1, StringSplitOptions.TrimEntries);
			var a1 = new string[count];
			int copyCount = Math.Min(a0.Length, count);
			Array.Copy(a0, a1, copyCount);
			if (copyCount < count)
				Array.Fill(a1, string.Empty, copyCount, count - copyCount);
			return a1;
		}


		/// <summary>
		/// Удаляет повторяющуюся подстроку в НАЧАЛЕ строки или заменяет ее на альтернативную подстроку.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetTrimStart(
			this string source,
			string trimString,
			string? replacement = null)
		{
			return source.ReplaceStart(trimString, replacement);
		}


		/// <summary>
		/// Удаляет повторяющуюся подстроку в КОНЦЕ строки или заменяет ее на альтернативную подстроку.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetTrimEnd(
			this string source,
			string trimString,
			string? replacement = null)
		{
			return source.ReplaceEnd(trimString, replacement);
		}


		/// <summary>
		/// Возвращает из строки подстроку, находящуюся между подстроками <paramref name="before"/>
		/// и <paramref name="after"/>.
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="before">Подстрока, идущая перед целевым фрагментом.</param>
		/// <param name="after">Подстрока, идущая после целевого фрагмента.</param>
		/// <param name="comparisonType">Правила сравнения строк.</param>
		/// <returns>Найденная подстрока или пустая строка, если условия не выполнены.</returns>
		public static string GetTag(
			this string? instance,
			string before,
			string after,
			StringComparison comparisonType = StringComparison.InvariantCulture)
		{
			if (string.IsNullOrEmpty(instance))
				return string.Empty;
			int i1 = string.IsNullOrEmpty(before)
				? 0 : instance.IndexOf(before, comparisonType);
			if (i1 < 0)
				return string.Empty;
			i1 += string.IsNullOrEmpty(before) ? 0 : before.Length;
			int i2 = string.IsNullOrEmpty(after)
				? instance.Length
				: instance.IndexOf(after, i1, comparisonType);
			if (i2 < 0)
				return string.Empty;
			return instance[i1..i2];
		}

	}

}
