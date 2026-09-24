// rev 2026-09-16

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public class ConsoleMenuItem(
		ConsoleKey key,
		string title,
		Action action)
	{
		public ConsoleKey Key { get; } = key;
		public string Title { get; } = title;
		public Action Action { get; } = action;
	}



	public class ConsoleMenu
	{

		/* ctors */


		public ConsoleMenu()
		{
		}


		public ConsoleMenu(
			string title)
			: this()
		{
			Title = title;
		}


		/* properties */


		public string? Title { get; set; }
		public bool IsExitRequested { get; set; } = false;


		/* readonly properties */


		public List<ConsoleMenuItem> Items { get; } = [];


		/* methods */


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(
			ConsoleMenuItem item)
		{
			Items.Add(item);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(
			ConsoleKey key,
			string title,
			Action action)
		{
			Items.Add(new ConsoleMenuItem(key, title, action));
		}


		public void Run()
		{
			IsExitRequested = false;
			while (!IsExitRequested)
			{
				Console.WriteLine();
				if (!string.IsNullOrEmpty(Title))
					Console.WriteLine($"{Title}:");
				foreach (var item1 in Items)
				{
					string s1 = item1.Key switch
					{
						>= ConsoleKey.A and <= ConsoleKey.Z
							=> item1.Key.ToString(),
						>= ConsoleKey.D0 and <= ConsoleKey.D9
							=> ((int)item1.Key - (int)ConsoleKey.D0).ToString(),
						_ => ((char)item1.Key).ToString()
					};
					Console.WriteLine($"[{s1}] — {item1.Title}");
				}
				Console.WriteLine("---------------------------");
				Console.Write("Введите требуемый пункт или нажмите Esc для выхода: ");
				var key1 = Console.ReadKey(intercept: true);
				Console.WriteLine();
				if (key1.Key == ConsoleKey.Escape)
				{
					IsExitRequested = true;
					break;
				}
				var match1 = Items.FirstOrDefault(i => i.Key == key1.Key);
				match1?.Action();
			}
		}

	}

}
