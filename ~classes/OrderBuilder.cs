// rev 2026-09-18

using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Представляет отдельный элемент сортировки (колонку и направление).
	/// </summary>
	public class OrderItem
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="OrderItem"/>
		/// на основе строкового определения (например, "Name" или "-Age").
		/// </summary>
		/// <param name="order">
		/// Строковое имя колонки, опционально начинающееся
		/// с дефиса '-' для сортировки по убыванию.
		/// </param>
		public OrderItem(
			string order)
		{
			if (!string.IsNullOrEmpty(order) && order[0] == '-')
			{
				IsDescending = true;
				Column = order[1..];
			}
			else
				Column = order ?? string.Empty;
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает имя свойства (колонки) для сортировки.
		/// </summary>
		public string Column { get; }

		/// <summary>
		/// Возвращает признак сортировки по убыванию.
		/// </summary>
		public bool IsDescending { get; }


		/* functions */


		/// <summary>
		/// Возвращает строковое представление элемента сортировки.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return $"{IsDescending.Make("-")}{Column}";
		}

	}



	/// <summary>
	/// Строит и инкапсулирует коллекцию элементов для управления порядком сортировки данных.
	/// </summary>
	public class OrderBuilder
	{

		private static readonly char[] _Sep1 = [';', ','];
		private readonly List<OrderItem> _items = [];
		private readonly OrderItem[] _itemsCache;


		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="OrderBuilder"/>
		/// на основе строки с перечислением полей сортировки.
		/// </summary>
		/// <param name="order">Строка с полями сортировки, разделенная запятыми
		/// или точкой с запятой (например, "Name,-Age").</param>
		public OrderBuilder(
			string order)
		{
			if (!string.IsNullOrEmpty(order))
			{
				var a1 = order.Split(_Sep1, StringSplitOptions.RemoveEmptyEntries);
				foreach (var item1 in a1)
					_items.Add(new OrderItem(item1));
			}
			_itemsCache = [.. _items];
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает массив элементов сортировки.
		/// </summary>
		public OrderItem[] Items
			=> _itemsCache;


		/* functions */


		/// <summary>
		/// Возвращает строковое представление всей цепочки сортировок, разделенное запятыми.
		/// </summary>
		public override string ToString()
		{
			return string.Join(",", _items.Select(x => x.ToString()));
		}


		/// <summary>
		/// Генерирует и возвращает форматированный псевдокод LINQ-цепочки методов
		/// сортировки для отладки или логирования.
		/// </summary>
		/// <param name="offset">
		/// Строковый отступ (префикс) перед методами сортировки
		/// (например, символы табуляции или пробелы).
		/// </param>
		public string GetLinqCode(
			string offset)
		{
			if (_items.Count == 0)
				return string.Empty;
			var sb1 = new StringBuilder();
			var s1 = (_items[0].IsDescending)
				? $"\n{offset}.OrderByDescending(x => x.{_items[0].Column})"
				: $"\n{offset}.OrderBy(x => x.{_items[0].Column})";
			sb1.Append(s1);
			for (int i1 = 1; i1 < _items.Count; i1++)
			{
				var item1 = _items[i1];
				var s2 = (item1.IsDescending)
					? $"\n{offset}.ThenByDescending(x => x.{item1.Column})"
					: $"\n{offset}.ThenBy(x => x.{item1.Column})";
				sb1.Append(s2);
			}
			return sb1.ToString();
		}

	}

}