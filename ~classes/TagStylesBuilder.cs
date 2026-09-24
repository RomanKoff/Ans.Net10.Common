// rev 2026-09-19

using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Строитель и контейнер инлаиновых CSS-стилей (атрибута style) для HTML-тегов.
	/// </summary>
	public class TagStylesBuilder
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр класса <see cref="TagStylesBuilder"/>.
		/// </summary>
		public TagStylesBuilder()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TagStylesBuilder"/>
		/// и разбирает переданную строку стилей.
		/// </summary>
		/// <param name="cssStyles">Строка CSS-стилей (например, "color:red;padding:10px;").</param>
		public TagStylesBuilder(
			string? cssStyles)
			: this()
		{
			Append(cssStyles);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает внутренний словарь уникальных CSS-свойств и их значений.
		/// </summary>
		public Dictionary<string, string> Items { get; } = [];


		/* methods */


		/// <summary>
		/// Добавляет стили из другого словаря, только если имя CSS-свойства отсутствует в текущей коллекции.
		/// </summary>
		public void ApplyOriginal(
			Dictionary<string, string>? dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				Items.TryAdd(item1.Key, item1.Value);
		}


		/// <summary>
		/// Разбирает строку стилей и добавляет те свойства, которых еще нет в текущей коллекции.
		/// </summary>
		public void ApplyOriginal(
			string? cssStyles)
		{
			if (string.IsNullOrEmpty(cssStyles))
				return;
			ApplyOriginal(_getDict(cssStyles));
		}


		/// <summary>
		/// Перезаписывает или добавляет стили из другого словаря в текущую коллекцию.
		/// </summary>
		public void Append(
			Dictionary<string, string>? dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				Items[item1.Key] = item1.Value;
		}


		/// <summary>
		/// Разбирает и добавляет инлайновые CSS-стили. Существующие свойства перезаписываются новыми значениями.
		/// </summary>
		public void Append(
			string? cssStyles)
		{
			if (string.IsNullOrEmpty(cssStyles))
				return;
			Append(_getDict(cssStyles));
		}


		/// <summary>
		/// Добавляет инлайновые CSS-стили, если переданное условие истинно.
		/// </summary>
		public void AppendIf(
			bool check,
			string? cssStyles)
		{
			if (check)
				Append(cssStyles);
		}


		/* functions */


		/// <summary>
		/// Формирует итоговую валидную строку инлайнового стиля для HTML-тега.
		/// </summary>
		/// <returns>Строка стилей вида "prop1:val1;prop2:val2;" или пустая строка, если стилей нет.</returns>
		public override string ToString()
		{
			if (Items.Count == 0)
				return string.Empty;
			var sb1 = new StringBuilder();
			foreach (var item1 in Items)
			{
				sb1.Append(item1.Key);
				sb1.Append(':');
				sb1.Append(item1.Value);
				sb1.Append(';');
			}
			return sb1.ToString();
		}


		/* privates */


		private static string[] _getItems(
			string cssStyles)
		{
			return cssStyles.Split(
				';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		}


		private static (string? key, string? value) _getParts(
			string cssStyle)
		{
			var parts1 = cssStyle.Split(':');
			return parts1.Length == 2
				? (parts1[0].Trim(), parts1[1].Trim())
				: (null, null);
		}


		private static Dictionary<string, string> _getDict(
			string cssStyles)
		{
			var dict1 = new Dictionary<string, string>();
			foreach (var item1 in _getItems(cssStyles))
			{
				var (key1, value1) = _getParts(item1);
				if (key1 != null && value1 != null)
					dict1[key1] = value1;
			}
			return dict1;
		}

	}

}
