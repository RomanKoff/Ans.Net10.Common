// rev 2026-09-10

using System.Buffers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Предоставляет вспомогательные свойства и методы
	/// для получения метаданных и путей текущего приложения.
	/// </summary>
	public static partial class SuppApp
	{

		/* readonly properties */


		/// <summary>
		/// Возвращает главную (входную) сборку приложения.
		/// </summary>
		public static Assembly? EntryAssembly
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Assembly.GetEntryAssembly();
		}


		/// <summary>
		/// Возвращает сборку, которая вызвала текущий метод.
		/// </summary>
		public static Assembly? CallingAssembly
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Assembly.GetCallingAssembly();
		}


		/// <summary>
		/// Возвращает или задает полный путь к текущему рабочему каталогу приложения.
		/// </summary>
		public static string CurrentDirectory
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Environment.CurrentDirectory;
		}


		/// <summary>
		/// Возвращает базовый каталог, используемый сборщиком подсистем для поиска сборок (каталог запуска).
		/// </summary>
		public static string BaseDirectory
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => AppDomain.CurrentDomain.BaseDirectory;
		}


		/// <summary>
		/// Возвращает предполагаемый путь к каталогу проекта Visual Studio. 
		/// Вычисляется как подъем на 4 уровня вверх от базового каталога. Результат кэшируется.
		/// </summary>
		public static string VSProjectPath
			=> field ??= _safeGetParent(BaseDirectory, 4);


		/// <summary>
		/// Возвращает имя каталога проекта Visual Studio. Результат кэшируется.
		/// </summary>
		public static string VSProjectName
			=> field ??= _getLastPathSegment(VSProjectPath);


		/// <summary>
		/// Возвращает предполагаемый путь к каталогу решения (Solution) Visual Studio. 
		/// Вычисляется как подъем на 5 уровней вверх от базового каталога. Результат кэшируется.
		/// </summary>
		public static string VSSolutionPath
			=> field ??= _safeGetParent(BaseDirectory, 5);


		/// <summary>
		/// Возвращает имя каталога решения (Solution) Visual Studio. Результат кэшируется.
		/// </summary>
		public static string VSSolutionName
			=> field ??= _getLastPathSegment(VSSolutionPath);


		/* privates */


		private static readonly char[] _SEP_DIR_CHARS = ['/', '\\'];
		private static readonly SearchValues<char> _SEP_DIR = SearchValues.Create(_SEP_DIR_CHARS);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string _getLastPathSegment(
		string path)
		{
			if (string.IsNullOrEmpty(path))
				return string.Empty;
			var span1 = path.AsSpan().TrimEnd(_SEP_DIR_CHARS);
			var i1 = span1.LastIndexOfAny(_SEP_DIR);
			if (i1 == -1)
				return span1.ToString();
			var result1 = span1[(i1 + 1)..];
			return result1.Contains(':')
				? string.Empty : result1.ToString();
		}


		private static string _safeGetParent(
			string path,
			int levels)
		{
			if (string.IsNullOrEmpty(path))
				return string.Empty;
			string s1 = path;
			for (int i1 = 0; i1 < levels; i1++)
			{
				string? parent1 = Path.GetDirectoryName(s1);
				if (string.IsNullOrEmpty(parent1))
					return s1;
				s1 = parent1;
			}
			return s1;
		}

	}

}
