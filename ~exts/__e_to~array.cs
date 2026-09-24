// rev 2026-09-08

using System.Buffers;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class __e_to
	{

		private static readonly char[] _SEP_ITEMS = [',', ';', '|'];
		private static readonly SearchValues<char> _SEP_ITEMS2 = SearchValues.Create(_SEP_ITEMS);


		/// <summary>
		/// Разбивает последовательность на блоки (массивы) заданного размера.
		/// </summary>
		/// <typeparam name="T">Тип элементов последовательности.</typeparam>
		/// <param name="source">Исходная последовательность элементов.</param>
		/// <param name="count">Максимальное количество элементов в одном блоке.</param>
		/// <returns>Последовательность массивов, содержащих элементы исходной коллекции.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<T[]> ToGrid<T>(
			this IEnumerable<T>? source,
			int count)
		{
			if (source == null || count < 1)
				return [];
			return source.Chunk(count);
		}


		/// <summary>
		/// Преобразует массив строк в массив целых чисел. 
		/// Если элемент не является числом, вместо него записывается 0.
		/// </summary>
		/// <param name="source">Исходный массив строк.</param>
		/// <returns>Массив целых чисел.</returns>
		public static int[] ToIntArray(
			this string[]? source)
		{
			if (source == null || source.Length == 0)
				return [];
			var a1 = new int[source.Length];
			for (var i1 = 0; i1 < source.Length; i1++)
				a1[i1] = source[i1].ToInt(0);
			return a1;
		}


		/// <summary>
		/// Разбивает строку по разделителям (запятая, точка с запятой, вертикальная черта) 
		/// и преобразует элементы в массив целых чисел без выделения промежуточных строк.
		/// </summary>
		/// <param name="source">Исходная строка с числами.</param>
		/// <returns>Массив целых чисел. Если строка пуста, возвращается пустой массив.</returns>
		public static int[] ToIntArray(
			this string? source)
		{
			if (string.IsNullOrEmpty(source))
				return [];
			var span1 = source.AsSpan();
			int count1 = 1;
			int offset1 = 0;
			while (true)
			{
				var i1 = span1[offset1..].IndexOfAny(_SEP_ITEMS2);
				if (i1 == -1)
					break;
				count1++;
				offset1 += i1 + 1;
			}
			var a1 = new int[count1];
			int i2 = 0;
			foreach (Range segment1 in span1.SplitAny(_SEP_ITEMS2))
				a1[i2++] = int.TryParse(span1[segment1], out int val1)
					? val1 : 0;
			return a1;
		}


		/// <summary>
		/// Разбивает строку по разделителям, удаляя пустые элементы и пробелы, 
		/// и преобразует элементы в массив целых чисел.
		/// </summary>
		/// <param name="source">Исходная строка с числами.</param>
		/// <returns>Массив целых чисел без пустых элементов.</returns>
		public static int[] ToIntArrayTrim(
			this string? source)
		{
			if (string.IsNullOrWhiteSpace(source))
				return [];
			var span1 = source.AsSpan();
			var a1 = new List<int>();
			foreach (Range segment1 in span1.SplitAny(_SEP_ITEMS2))
			{
				var chunk1 = span1[segment1].Trim();
				if (chunk1.IsEmpty)
					continue;
				a1.Add(int.TryParse(chunk1, out int val1) ? val1 : 0);
			}
			return [.. a1];
		}

	}

}
