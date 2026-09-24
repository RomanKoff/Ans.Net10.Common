// rev 2026-09-19

using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Коллекция параметров для формирования строки URL-запроса (Query String) с фильтрацией пустых значений.
	/// </summary>
	public class ParamsCollection
	{

		private readonly Dictionary<string, string> _items;


		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр класса <see cref="ParamsCollection"/>.
		/// </summary>
		public ParamsCollection()
		{
			_items = [];
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ParamsCollection"/>
		/// с заданной емкостью и компаратором ключей.
		/// </summary>
		/// <param name="capacity">Начальная емкость словаря.</param>
		/// <param name="comparer">
		/// Компаратор для сравнения имен параметров (например, регистронезависимый).
		/// </param>
		public ParamsCollection(
			int capacity,
			IEqualityComparer<string> comparer)
		{
			_items = new Dictionary<string, string>(capacity, comparer);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает внутренний словарь элементов параметров.
		/// </summary>
		public Dictionary<string, string> Items
			=> _items;


		/* methods */


		/// <summary>
		/// Добавляет или обновляет строковое значение параметра.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Строковое значение параметра.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			string value)
		{
			_items[name] = value;
		}


		/// <summary>
		/// Добавляет параметр со значением "1", если переданное логическое значение истинно.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Логическое значение.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			bool value)
		{
			if (value)
				Append(name, "1");
		}


		/// <summary>
		/// Добавляет параметр, если числовое значение не равно 0.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Числовое значение.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			int value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		/// <summary>
		/// Добавляет параметр, если числовое значение не равно 0.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Числовое значение.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			long value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		/// <summary>
		/// Добавляет параметр, если числовое значение не равно 0.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Числовое значение.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			double value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		/// <summary>
		/// Добавляет параметр, если числовое значение не равно 0.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Числовое значение.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			float value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		/// <summary>
		/// Добавляет параметр, если числовое значение не равно 0.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Числовое значение.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			decimal value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		/// <summary>
		/// Добавляет параметр даты и времени в универсальном формате (исходя из "u"), если значение задано.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение даты и времени или <see langword="null"/>.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			DateTime? value)
		{
			if (value != null)
				Append(name, value.Value.ToString("u"));
		}


		/// <summary>
		/// Добавляет параметр даты в формате "yyyy-MM-dd", если значение задано.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение даты или <see langword="null"/>.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			DateOnly? value)
		{
			if (value != null)
				Append(name, value.Value.ToString("yyyy-MM-dd"));
		}


		/// <summary>
		/// Добавляет параметр времени в 24-часовом формате "HH:mm:ss", если значение задано.
		/// </summary>
		/// <param name="name">Имя параметра.</param>
		/// <param name="value">Значение времени или <see langword="null"/>.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			TimeOnly? value)
		{
			if (value != null)
				Append(name, value.Value.ToString("HH:mm:ss"));
		}


		/* functions */


		/// <summary>
		/// Преобразует коллекцию параметров в валидную строку HTTP-запроса, начинающуюся с символа '?'.
		/// </summary>
		/// <returns>
		/// Строка запроса вида "?param1=val1&amp;param2=val2" или пустая строка, если параметров нет.
		/// </returns>
		public override string ToString()
		{
			if (_items.Count == 0)
				return string.Empty;
			var sb1 = new StringBuilder();
			sb1.Append('?');
			bool isFirst1 = true;
			foreach (var item1 in _items)
			{
				if (!isFirst1)
					sb1.Append('&');
				sb1.Append(item1.Key);
				sb1.Append('=');
				sb1.Append(item1.Value);
				isFirst1 = false;
			}
			return sb1.ToString();
		}

	}

}
