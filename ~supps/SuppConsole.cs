using System.Reflection;

namespace Ans.Net10.Common
{

	public static class SuppConsole
	{

		/* properties */


		public static int CursorLeft { get; set; }
		public static int CursorTop { get; set; }
		public static int Counter { get; set; }


		/* methods */


		public static void WriteDict<T>(
			string title,
			IDictionary<string, T> dictionary,
			Func<KeyValuePair<string, T>, string> itemLog)
			where T : class
		{
			var c1 = dictionary.Count;
			Console.WriteLine($"{title}: {c1}");
			foreach (var item1 in dictionary)
			{
				Console.WriteLine(itemLog(item1));
			}
			Console.WriteLine();
		}


		public static void WriteItems<T>(
			string title,
			IEnumerable<T> items,
			Func<T, string> itemLog,
			Action<T> itemAction)
			where T : class
		{
			var c1 = items.Count();
			Console.WriteLine($"{title}: {c1}");
			foreach (var item1 in items)
			{
				Console.WriteLine(itemLog(item1));
				itemAction?.Invoke(item1);
			}
			Console.WriteLine();
		}


		public static void InputClear()
		{
			while (Console.KeyAvailable)
				_ = Console.ReadKey(true);
		}


		public static void SetCounter(
			int counter)
		{
			CursorSavePos();
			Counter = counter;
		}


		public static void CounterUp()
		{
			Counter++;
			CursorRestopePos();
			Console.Write(Counter);
			Console.WriteLine(" ");
		}


		public static void CounterDown()
		{
			Counter--;
			CursorRestopePos();
			Console.Write(Counter);
			Console.WriteLine(" ");
		}


		/// <summary>
		/// Сохраняет текущую позицию курсора
		/// </summary>
		public static void CursorSavePos()
		{
			CursorLeft = Console.CursorLeft;
			CursorTop = Console.CursorTop;
		}


		/// <summary>
		/// Восстанавливает сохраненную ранее позицию курсора
		/// </summary>
		public static void CursorRestopePos()
		{
			Console.SetCursorPosition(CursorLeft, CursorTop);
		}


		/// <summary>
		/// Переводит курсов в начало текущей строки
		/// </summary>
		public static void CursorToBeginLine()
		{
			Console.SetCursorPosition(0, Console.CursorTop);
		}


		public static void ClearLine()
		{
			CursorToBeginLine();
			Console.Write(new string(' ', Console.BufferWidth));
			CursorToBeginLine();
		}


		public static void WriteFreeze(
			string template,
			params object[] args)
		{
			CursorSavePos();
			Console.Write(template, args);
			CursorRestopePos();
		}


		public static void Write(
			ConsoleColor fgColor,
			string text)
		{
			var save1 = Console.ForegroundColor;
			Console.ForegroundColor = fgColor;
			Console.Write(text);
			Console.ForegroundColor = save1;
		}


		public static void Write(
			bool expression,
			ConsoleColor fgColor,
			string text)
		{
			if (expression)
				Write(fgColor, text);
			else
				Console.Write(text);
		}


		public static void WriteLineParam(
			string title,
			string value,
			ConsoleColor? fgColor = null)
		{
			var save1 = Console.ForegroundColor;
			Console.Write($"{title}: ");
			Console.ForegroundColor = fgColor ?? ConsoleColor.Green;
			Console.Write(value);
			Console.ForegroundColor = save1;
			Console.WriteLine();
		}


		public static void Start(
			string title)
		{
			Console.WriteLine(title);
			BreakLineW();
			Console.WriteLine();
		}


		public static void Start()
		{
			Start(Assembly.GetCallingAssembly().GetName().Name);
		}


		public static void SectionStart(
			string title)
		{
			Console.WriteLine(title);
			BreakLine();
			Console.WriteLine();
		}


		public static void SectionEnd(
			string title = null)
		{
			Console.WriteLine();
			if (!string.IsNullOrEmpty(title))
				Console.WriteLine(title);
			BreakLine();
			Console.WriteLine();
		}


		public static void End()
		{
			Console.WriteLine();
			BreakLineW();
			Console.WriteLine(Resources.Common.Text_PressAnyKeyToExit);
			_ = Console.ReadKey();
		}


		public static void BreakLine()
		{
			Console.WriteLine("-".MakeRepeats(80));
		}


		public static void BreakLineW()
		{
			Console.WriteLine("=".MakeRepeats(80));
		}


		/* functions */


		public static ConsoleKeyInfo ReadKey()
		{
			InputClear();
			while (!Console.KeyAvailable) { }
			return Console.ReadKey(true);
		}


		public static bool GetCase(
			string query)
		{
			Console.Write($"{query} (y|n)? ");
			InputClear();
			while (!Console.KeyAvailable) { }
			var key1 = Console.ReadKey(true);
			Console.WriteLine();
			return key1.KeyChar is 'y' or 'Y';
		}

	}

}
