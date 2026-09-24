// rev 2026-09-16

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	public static partial class SuppLangRu
	{

		private static readonly SearchValues<char> _umlautSearch = SearchValues.Create("ёЁ");


		/// <summary>
		/// Заменяет букву 'ё' и 'Ё' на 'е' и 'Е'.
		/// </summary>
		/// <param name="value">Исходный символ.</param>
		/// <returns>Символ 'е'/'Е', если на входе была буква с умлаутом; иначе исходный символ.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static char GetFixUmlautRu(
			char value)
		{
			return value switch
			{
				'ё' => 'е',
				'Ё' => 'Е',
				_ => value
			};
		}


		/// <summary>
		/// Возвращает новую строку, в которой все буквы 'ё' и 'Ё' заменены на 'е' и 'Е'.
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Очищенная от умлаутов строка. Если входная строка пуста или null, возвращает пустую строку.</returns>
		public static string GetFixUmlautRu(
			string? source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			if (source.AsSpan().IndexOfAny(_umlautSearch) == -1)
				return source;
			return string.Create(source.Length, source, static (span1, src1) =>
			{
				for (int i1 = 0; i1 < span1.Length; i1++)
					span1[i1] = GetFixUmlautRu(src1[i1]);
			});
		}


		/// <summary>
		/// Выполняет быструю замену всех букв 'ё' и 'Ё' на 'е' и 'Е' непосредственно в исходном объекте <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="source">Исходный StringBuilder для модификации по месту.</param>
		/// <returns>Тот же экземпляр StringBuilder с выполненными заменами. Если входной объект null, возвращает null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static StringBuilder? FixUmlautRu(
			StringBuilder? source)
		{
			if (source == null || source.Length == 0)
				return source;
			return source
				.Replace('ё', 'е')
				.Replace('Ё', 'Е');
		}


		/// <summary>
		/// Возвращает НОВЫЙ экземпляр <see cref="StringBuilder"/>, в котором все буквы 'ё' и 'Ё' заменены на 'е' и 'Е'. Исходный объект остается неизменным.
		/// </summary>
		/// <param name="source">Исходный StringBuilder.</param>
		/// <returns>Новый StringBuilder с выполненными заменами.</returns>
		public static StringBuilder GetFixUmlautRu(
			StringBuilder? source)
		{
			if (source == null)
				return new StringBuilder();
			var sb1 = new StringBuilder(source.Length);
			foreach (ReadOnlyMemory<char> chunk1 in source.GetChunks())
			{
				var span1 = chunk1.Span;
				for (int i1 = 0; i1 < span1.Length; i1++)
					sb1.Append(GetFixUmlautRu(span1[i1]));
			}
			return sb1;
		}


		/// <summary>
		/// Заменяет кириллический знак номера на международный типографический эквивалент (№ --> Nº).
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Строка с замененным знаком номера. Если входная строка пуста или null, возвращает пустую строку.</returns>
		public static string GetFixNumberRu(
			string? source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			return source.Replace("№", "Nº");
		}


		/// <summary>
		/// Возвращает экземпляр <see cref="StringBuilder"/>, в котором кириллический
		/// знак номера заменен на международный эквивалент (№ --> Nº). 
		/// Если знак «№» отсутствует, в целях оптимизации возвращается исходный объект без выделения памяти.
		/// </summary>
		/// <param name="source">Исходный StringBuilder.</param>
		/// <returns>StringBuilder с выполненными заменами.</returns>
		public static StringBuilder GetFixNumberRu(
			StringBuilder? source)
		{
			if (source == null)
				return new StringBuilder();
			int i1 = 0;
			foreach (ReadOnlyMemory<char> chunk1 in source.GetChunks())
				i1 += chunk1.Span.Count('№');
			if (i1 == 0)
				return source;
			var sb1 = new StringBuilder(source.Length + i1);
			foreach (ReadOnlyMemory<char> chunk1 in source.GetChunks())
				sb1.Append(chunk1.Span);
			return sb1.Replace("№", "Nº");
		}

	}

}
