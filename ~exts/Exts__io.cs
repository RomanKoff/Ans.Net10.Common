// rev 2026-09-17

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class Exts__io
	{

		private static readonly char[] _Sep_Exts = [';', ','];


		/// <summary>
		/// Возвращает перечисление файлов в каталоге, расширения которых
		/// соответствуют указанному списку (регистронезависимо).
		/// </summary>
		/// <param name="directoryInfo">Информационный объект каталога.</param>
		/// <param name="extensions">Список целевых расширений файлов (например, ".txt", ".jpg").</param>
		public static IEnumerable<FileInfo> GetFilesByExtensions(
			this DirectoryInfo directoryInfo,
			params ReadOnlySpan<string> extensions)
		{
			ArgumentNullException.ThrowIfNull(directoryInfo);
			var a1 = new string[extensions.Length];
			for (int i1 = 0; i1 < extensions.Length; i1++)
				a1[i1] = extensions[i1];
			return directoryInfo.EnumerateFiles()
				.Where(x => a1.Contains(x.Extension, StringComparer.OrdinalIgnoreCase));
		}


		/// <summary>
		/// Возвращает перечисление файлов в каталоге, расширения которых
		/// соответствуют строке со списком расширений, разделенных точкой с запятой или запятой.
		/// </summary>
		/// <param name="directoryInfo">Информационный объект каталога.</param>
		/// <param name="extensions">Строка с расширениями, разделенная символами ';' или ',' (например, ".txt;.jpg").</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<FileInfo> GetFilesByExtensions(
			this DirectoryInfo directoryInfo,
			string extensions)
		{
			ArgumentNullException.ThrowIfNull(extensions);
			return GetFilesByExtensions(
				directoryInfo,
				extensions.Split(_Sep_Exts, StringSplitOptions.RemoveEmptyEntries));
		}

	}

}
