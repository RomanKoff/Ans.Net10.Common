// rev 2026-09-20

namespace Ans.Net10.Common
{

	/// <summary>
	/// Специализированный словарь стилизаторов тегов, поддерживающий инициализацию 
	/// из массива сериализованных строк формата "ИмяКлюча|Классы|Стили".
	/// </summary>
	public class DictTagStylers
		: Dictionary<string, TagStyler>
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="DictTagStylers"/> на основе массива строк.
		/// Каждая строка должна иметь формат "ИмяКлюча|Классы|Стили".
		/// Пустые элементы автоматически игнорируются.
		/// </summary>
		/// <param name="serialization">Массив сериализованных строк для наполнения словаря.</param>
		public DictTagStylers(
			params string[] serialization)
			: base(serialization?.Length ?? 0)
		{
			if (serialization == null)
				return;
			foreach (var item1 in serialization)
			{
				if (string.IsNullOrWhiteSpace(item1))
					continue;
				var a1 = item1.SplitFix("|", 3);
				if (!ContainsKey(a1[0]))
					Add(a1[0], new TagStyler(a1[1], a1[2]));
			}
		}

	}

}
