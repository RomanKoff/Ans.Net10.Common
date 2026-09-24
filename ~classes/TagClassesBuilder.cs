// rev 2026-09-19

using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Строитель и контейнер для группировки, слияния и генерации CSS-классов HTML-тегов.
	/// </summary>
	public class TagClassesBuilder
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр класса <see cref="TagClassesBuilder"/>.
		/// </summary>
		public TagClassesBuilder()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TagClassesBuilder"/>
		/// и наполняет его из строки классов.
		/// </summary>
		/// <param name="cssClasses">Строка CSS-классов, разделенная пробелами.</param>
		public TagClassesBuilder(
			string? cssClasses)
			: this()
		{
			Append(cssClasses);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает внутренний словарь сгруппированных префиксов классов и их суффиксов.
		/// </summary>
		public Dictionary<string, string[]> Items { get; } = [];


		/* methods */


		/// <summary>
		/// Добавляет элементы из другого словаря классов, только если
		/// префикс класса отсутствует в текущей коллекции.
		/// </summary>
		public void ApplyOriginal(
			Dictionary<string, string[]>? dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				Items.TryAdd(item1.Key, item1.Value);
		}


		/// <summary>
		/// Разбирает строку классов и добавляет те, чьи базовые
		/// префиксы отсутствуют в текущей коллекции.
		/// </summary>
		public void ApplyOriginal(
			string? cssClasses)
		{
			if (string.IsNullOrEmpty(cssClasses))
				return;
			ApplyOriginal(_getDict(cssClasses));
		}


		/// <summary>
		/// Перезаписывает или добавляет элементы из другого словаря классов в текущую коллекцию.
		/// </summary>
		public void Append(
			Dictionary<string, string[]>? dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				Items[item1.Key] = item1.Value;
		}


		/// <summary>
		/// Разбирает и добавляет CSS-классы в текущую коллекцию. Существующие префиксы перезаписываются.
		/// </summary>
		public void Append(
			string? cssClasses)
		{
			if (string.IsNullOrEmpty(cssClasses))
				return;
			Append(_getDict(cssClasses));
		}


		/// <summary>
		/// Добавляет CSS-классы в текущую коллекцию, если переданное условие истинно.
		/// </summary>
		public void AppendIf(
			bool check,
			string? cssClasses)
		{
			if (check)
				Append(cssClasses);
		}


		/* functions */


		/// <summary>
		/// Собирает все сгруппированные CSS-классы в единую валидную строку для атрибута class.
		/// </summary>
		/// <returns>
		/// Строка классов, разделенная пробелами, или <see langword="null"/>, если коллекция пуста.
		/// </returns>
		public override string? ToString()
		{
			if (Items.Count == 0)
				return null;
			var sb1 = new StringBuilder();
			foreach (var item1 in Items)
				foreach (var value1 in item1.Value)
				{
					if (sb1.Length > 0)
						sb1.Append(' ');
					sb1.Append(item1.Key);
					sb1.Append(value1);
				}
			return sb1.ToString();
		}


		/* privates */


		private static string[] _getItems(
			string cssClasses)
		{
			return cssClasses.Split(
				' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		}


		private static (string key, string value) _getParts(
			string cssClass)
		{
			var i1 = cssClass.LastIndexOf('-');
			if (i1 == -1 || i1 == cssClass.Length - 1)
				return (cssClass, string.Empty);
			i1++;
			return (cssClass[..i1], cssClass[i1..]);
		}


		private static Dictionary<string, string[]> _getDict(
			string cssClasses)
		{
			var dict1 = new Dictionary<string, string[]>();
			foreach (var item1 in _getItems(cssClasses))
			{
				var (key1, value1) = _getParts(item1);
				if (dict1.TryGetValue(key1, out var existingArray1))
					dict1[key1] = existingArray1.GetArrayAdd(value1);
				else
					dict1[key1] = [value1];
			}
			return dict1;
		}

	}

}
