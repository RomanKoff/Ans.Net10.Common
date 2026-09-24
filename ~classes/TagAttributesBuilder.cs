// rev 2026-09-19

using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Строитель и контейнер свободных HTML-атрибутов (например, id, data-*, href, disabled) для HTML-тегов.
	/// </summary>
	public class TagAttributesBuilder
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр класса <see cref="TagAttributesBuilder"/>.
		/// </summary>
		public TagAttributesBuilder()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TagAttributesBuilder"/>
		/// и разбирает переданную строку атрибутов.
		/// </summary>
		/// <param name="serialization">
		/// Строка сериализованных атрибутов (например, "id=\"main\" data-id=\"12\" disabled").
		/// </param>
		public TagAttributesBuilder(
			string? serialization)
			: this()
		{
			Append(serialization);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает внутренний словарь уникальных атрибутов и их значений.
		/// </summary>
		public Dictionary<string, string?> Items { get; } = [];


		/* methods */


		/// <summary>
		/// Разбирает и добавляет HTML-атрибуты. Существующие атрибуты перезаписываются новыми значениями.
		/// </summary>
		public void Append(
			string? serialization)
		{
			if (string.IsNullOrEmpty(serialization))
				return;
			foreach (var item1 in _getItems(serialization))
			{
				var (key1, value1) = _getParts(item1);
				Items[key1] = value1;
			}
		}


		/// <summary>
		/// Добавляет HTML-атрибуты, если переданное условие истинно.
		/// </summary>
		public void AppendIf(
			bool check,
			string? serialization)
		{
			if (check)
				Append(serialization);
		}


		/* functions */


		/// <summary>
		/// Формирует строку HTML-атрибутов, готовую для рендеринга внутри открывающего тега.
		/// Всегда начинается с пробела.
		/// </summary>
		/// <returns>
		/// Строка атрибутов вида " id=\"val\" disabled" или пустая строка, если атрибутов нет.
		/// </returns>
		public override string ToString()
		{
			if (Items.Count == 0)
				return string.Empty;
			var sb1 = new StringBuilder();
			foreach (var item1 in Items)
			{
				sb1.Append(' ');
				sb1.Append(item1.Key);
				if (item1.Value != null)
				{
					sb1.Append("=\"");
					sb1.Append(item1.Value);
					sb1.Append('"');
				}
			}
			return sb1.ToString();
		}


		/* privates */


		private static string[] _getItems(
			string serialization)
		{
			return serialization.Split(
				' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		}


		private static (string key, string? value) _getParts(
			string serialization)
		{
			var i1 = serialization.LastIndexOf('=');
			if (i1 == -1)
				return (serialization, null);
			var key1 = serialization[..i1].Trim();
			var value1 = serialization[(i1 + 1)..].Trim().Trim('"');
			return (key1, value1);
		}

	}

}
