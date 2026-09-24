// rev 2026-09-15

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для работы с регулярными выражениями.
	/// </summary>
	public static partial class SuppRegex
	{

		/// <summary>
		/// Экранирует спецсимволы в строке, заменяя их эквивалентами, безопасными для использования в регулярных выражениях.
		/// </summary>
		/// <param name="source">Исходная строка, содержащая спецсимволы регулярных выражений, или null.</param>
		/// <returns>Строка с экранированными спецсимволами. Если передана пустая строка или null, возвращается исходное значение.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string Escape(
			string? source)
		{
			if (string.IsNullOrEmpty(source))
				return source ?? string.Empty;
			return Regex.Escape(source);
		}

	}

}
