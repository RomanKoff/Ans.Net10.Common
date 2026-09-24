// rev 2026-09-11

using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{


	/// <summary>
	/// Вспомогательный класс для работы
	/// с консольным UI.
	/// </summary>
	public static class SuppConsole
	{

		private static readonly string _lineV = "-".MakeRepeats(80);
		private static readonly string _lineW = "=".MakeRepeats(80);

		private static int _cursorLeft;
		private static int _cursorTop;
		private static int _counter;


		public static void AppStart(
			string title)
		{
			Console.WriteLine();
			WriteColor(ConsoleColor.Green, _lineW);
			Console.WriteLine();
			WriteColor(ConsoleColor.Green, title);
			Console.WriteLine();
			WriteColor(ConsoleColor.Green, _lineW);
			Console.WriteLine();
			Console.WriteLine();
		}


		public static void AppStart()
		{
			AppStart(Assembly.GetCallingAssembly().GetName().Name ?? "App");
		}


		public static void AppEnd()
		{
			Console.WriteLine();
			WriteColor(ConsoleColor.Green, _lineW);
			Console.WriteLine();
			Console.WriteLine(Resources.Common.Text_PressAnyKeyToExit);
			_ = Console.ReadKey(true);
			Console.WriteLine();
		}


		public static void SectionStart(
			string title)
		{
			Console.WriteLine();
			WriteColor(ConsoleColor.Yellow, _lineV);
			Console.WriteLine();
			WriteColor(ConsoleColor.Yellow, title);
			Console.WriteLine();
			WriteColor(ConsoleColor.Yellow, _lineV);
			Console.WriteLine();
			Console.WriteLine();
		}


		public static void PartStart(
			string title)
		{
			Console.WriteLine();
			WriteColor(ConsoleColor.DarkYellow, $"--- {title} ---");
			Console.WriteLine();
			Console.WriteLine();
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InputClear()
		{
			while (Console.KeyAvailable)
				_ = Console.ReadKey(true);
		}


		public static ConsoleKeyInfo ReadKey()
		{
			InputClear();
			return Console.ReadKey(true);
		}


		/*--- CURSOR ---*/


		/// <summary>
		/// Сохраняет текущую позицию курсора.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CursorSavePos()
		{
			_cursorLeft = Console.CursorLeft;
			_cursorTop = Console.CursorTop;
		}


		/// <summary>
		/// Восстанавливает сохраненную ранее позицию курсора.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CursorRestorePos()
		{
			Console.SetCursorPosition(_cursorLeft, _cursorTop);
		}


		/// <summary>
		/// Переводит курсор в начало текущей строки.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CursorToBeginLine()
		{
			Console.SetCursorPosition(0, Console.CursorTop);
		}


		/*--- COUNTER ---*/


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetCounter(
			int counter)
		{
			CursorSavePos();
			_counter = counter;
		}


		public static void CounterUp()
		{
			_counter++;
			CursorRestorePos();
			Console.Write(_counter);
			Console.WriteLine(" ");
		}


		public static void CounterDown()
		{
			_counter--;
			CursorRestorePos();
			Console.Write(_counter);
			Console.WriteLine(" ");
		}


		/*--- WRITE ---*/


		/// <summary>
		/// Возвращает строку для вывода на основе коллекции
		/// </summary>
		public static string ArrToText<T>(
			IEnumerable<T>? items)
		{
			if (items == null)
				return string.Empty;
			var f1 = typeof(T) == typeof(string) ? "\"{0}\"" : null;
			return items.MakeFromCollection(x => x?.ToString(), "[{0}]", f1, ", ");
		}


		/// <summary>
		/// Очищает текущую строку консоли.
		/// </summary>
		public static void ClearLine()
		{
			CursorToBeginLine();
			int width = Console.BufferWidth;
			if (width <= 256)
			{
				Span<char> buffer = stackalloc char[width];
				buffer.Fill('\x20');
				Console.Write((ReadOnlySpan<char>)buffer);
			}
			else
			{
				string bufferStr = string.Create(width, '\x20', static (span, space) => span.Fill(space));
				Console.Write(bufferStr);
			}
			CursorToBeginLine();
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BreakLine()
		{
			Console.WriteLine(_lineV);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BreakLineW()
		{
			Console.WriteLine(_lineW);
		}


		/// <summary>
		/// Производит вывод в консоль с заданным цветом фона.
		/// </summary>
		public static void WriteColor(
			ConsoleColor fgColor,
			string text)
		{
			var save1 = Console.ForegroundColor;
			Console.ForegroundColor = fgColor;
			Console.Write(text);
			Console.ForegroundColor = save1;
		}


		/// <summary>
		/// Производит вывод в консоль с заданным цветом фона,
		/// если условие верно.
		/// </summary>
		public static void WriteColor(
			bool expression,
			ConsoleColor fgColor,
			string text)
		{
			if (expression)
				WriteColor(fgColor, text);
			else
				Console.Write(text);
		}


		/// <summary>
		/// Оформленный вывод параметра (имя: значение)
		/// </summary>
		public static void WriteLineParam(
			string title,
			object? value,
			ConsoleColor? fgColor = null)
		{
			var save1 = Console.ForegroundColor;
			Console.Write($"{title}: ");
			Console.ForegroundColor = fgColor ?? ConsoleColor.Green;
			if (value != null)
				Console.Write(SuppReflection.GetCSharpValue(value, true));
			Console.ForegroundColor = save1;
			Console.WriteLine();
		}


		public static void WriteDict<T>(
			string title,
			IDictionary<string, T> dictionary,
			Func<KeyValuePair<string, T>, string> itemLog)
			where T : class
		{
			int count1 = dictionary.Count;
			Console.WriteLine($"{title}: {count1}");
			foreach (var item1 in dictionary)
				Console.WriteLine(itemLog(item1));
			Console.WriteLine();
		}


		public static void WriteItems<T>(
			string title,
			IEnumerable<T> items,
			Func<T, string> itemLog,
			Action<T> itemAction)
			where T : class
		{
			int count1 = items switch
			{
				ICollection<T> c => c.Count,
				IReadOnlyCollection<T> rc => rc.Count,
				_ => items.Count()
			};
			Console.WriteLine($"{title}: {count1}");
			foreach (var item1 in items)
			{
				Console.WriteLine(itemLog(item1));
				itemAction?.Invoke(item1);
			}
			Console.WriteLine();
		}


		/// <summary>
		/// Производит вывод в консоль без изменерния позиции курсора.
		/// </summary>
		public static void WriteFreeze(
			string template,
			params object[] args)
		{
			CursorSavePos();
			Console.Write(template, args);
			CursorRestorePos();
		}


		/*--- MENU ---*/


		public static void MakeMenu(
			IEnumerable<ConsoleMenuItem> items)
		{
			var menu1 = new ConsoleMenu();
			foreach (var item1 in items)
				menu1.Add(item1);
			menu1.Run();
		}


		public static bool GetCase(
			string query)
		{
			Console.Write($"{query} (y|n)? ");
			InputClear();
			var key1 = Console.ReadKey(true);
			Console.WriteLine();
			return key1.KeyChar is 'y' or 'Y';
		}

	}

}
