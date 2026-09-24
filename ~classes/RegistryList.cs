// rev 2026-09-20

using System.Resources;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Определяет режимы отображения элементов реестра в пользовательском интерфейсе.
	/// </summary>
	public enum RegistryModeEnum
	{
		/// <summary>
		/// Автоматический выбор режима на основе анализа данных.
		/// </summary>
		Auto,

		/// <summary>
		/// Отображение в виде группы элементов ввода (радиокнопки/чекбоксы).
		/// </summary>
		Inputs,

		/// <summary>
		/// Отображение в виде выпадающего списка выбора (Select/Dropdown).
		/// </summary>
		Select
	}



	/// <summary>
	/// Предоставляет данные для событий, связанных с операциями
	/// над одиночным элементом реестра <see cref="RegistryItem"/>.
	/// </summary>
	public class RegistryItemEventArgs(
		RegistryItem item)
		: EventArgs
	{
		/// <summary>
		/// Возвращает связанный с событием элемент реестра.
		/// </summary>
		public RegistryItem Item
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		} = item;
	}



	/// <summary>
	/// Представляет делегат для обработки событий, оперирующих элементами реестра.
	/// </summary>
	/// <param name="sender">Источник события.</param>
	/// <param name="e">Аргументы события, содержащие элемент реестра.</param>
	public delegate void RegistryItemEventHandler(
		object sender,
		RegistryItemEventArgs e);



	/// <summary>
	/// Представляет специализированный список элементов реестра
	/// с поддержкой событийной модели, фильтрации и иерархических операций.
	/// </summary>
	public class RegistryList
	{

		private readonly List<RegistryItem> _items = [];


		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр класса <see cref="RegistryList"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RegistryList()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="RegistryList"/> на основе коллекции элементов.
		/// </summary>
		/// <param name="items">Коллекция элементов для добавления в список.</param>
		public RegistryList(
			IEnumerable<RegistryItem> items)
			: this()
		{
			if (items != null)
				_items.AddRange(items.Where(x => x != null));
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="RegistryList"/>
		/// на основе массива элементов.
		/// </summary>
		/// <param name="items">Массив элементов для добавления в список.</param>
		public RegistryList(
			params RegistryItem[] items)
			: this(items?.AsEnumerable()!)
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="RegistryList"/>,
		/// десериализуя его из единой строки.
		/// </summary>
		/// <param name="serialization">Строка сериализации реестра в виде "item1;item2;...".</param>
		public RegistryList(
			string serialization)
			: this()
		{
			FillFromString(serialization);
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="RegistryList"/>
		/// на основе массива сериализованных строк.
		/// </summary>
		/// <param name="items">Массив строк, каждая из которых представляет отдельный элемент.</param>
		public RegistryList(
			params string[] items)
			: this()
		{
			FillFromString(items);
		}


		/* events */


		/// <summary>
		/// Наступает после успешного добавления нового элемента в список реестра.
		/// </summary>
		public event RegistryItemEventHandler? AddedItem;


		/* readonly properties */


		/// <summary>
		/// Возвращает перечисляемую коллекцию всех элементов реестра в текущем списке.
		/// </summary>
		public IEnumerable<RegistryItem> Items
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _items;
		}


		/// <summary>
		/// Возвращает значение, указывающее, содержит ли текущий список хотя бы один элемент.
		/// </summary>
		public bool HasItems
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _items.Count > 0;
		}


		/* methods */


		/// <summary>
		/// Десериализует реестр из коллекции строк.
		/// </summary>
		/// <param name="items">Коллекция строк элементов.</param>
		public void FillFromString(
			IEnumerable<string> items)
		{
			_items.Clear();
			if (items == null)
				return;
			bool hasNull1 = false;
			bool hasAll1 = false;
			foreach (var item1 in items)
			{
				if (string.IsNullOrEmpty(item1))
					continue;
				if (item1 == "!")
				{
					if (!hasNull1)
					{
						AddNullItem();
						hasNull1 = true;
					}
				}
				else if (item1 == "*")
				{
					if (!hasAll1)
					{
						AddAllItems();
						hasAll1 = true;
					}
				}
				else
				{
					Add(new RegistryItem(item1));
				}
			}
		}


		/// <summary>
		/// Десериализует реестр из массива строк.
		/// </summary>
		/// <param name="items">Массив строк элементов.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FillFromString(
			params string[] items)
		{
			if (items == null)
			{
				_items.Clear();
				return;
			}
			FillFromString(items.AsEnumerable());
		}


		/// <summary>
		/// Десериализует реестр из единой строки с разделителями "item1;item2;...".
		/// </summary>
		/// <param name="serialization">Строка сериализации реестра.</param>
		public void FillFromString(
			string serialization)
		{
			_items.Clear();
			if (string.IsNullOrEmpty(serialization))
				return;
			var masked1 = serialization.Replace("\\;", RegistryItem.MASK_semicolon);
			var tokens1 = masked1.Split(';', StringSplitOptions.RemoveEmptyEntries);
			var unmaskedTokens1 = tokens1.Select(t => t.Replace(RegistryItem.MASK_semicolon, "\\;"));
			FillFromString(unmaskedTokens1);
		}


		/// <summary>
		/// Безопасно вызывает событие <see cref="AddedItem"/>.
		/// </summary>
		/// <param name="e">Аргументы события, содержащие добавленный элемент.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void OnAddedItem(
			RegistryItemEventArgs e)
		{
			AddedItem?.Invoke(this, e);
		}


		/// <summary>
		/// Добавляет готовый элемент в конец списка реестра и генерирует событие добавления.
		/// </summary>
		/// <param name="item">Элемент реестра для добавления.</param>
		public void Add(
			RegistryItem item)
		{
			if (item == null)
				return;
			_items.Add(item);
			OnAddedItem(new RegistryItemEventArgs(item));
		}


		/// <summary>
		/// Вставляет элемент в список реестра по указанному индексу и генерирует событие добавления.
		/// </summary>
		/// <param name="index">Индекс для вставки элемента.</param>
		/// <param name="item">Элемент реестра для вставки.</param>
		public void Insert(
			int index,
			RegistryItem item)
		{
			if (item == null)
				return;
			_items.Insert(index, item);
			OnAddedItem(new RegistryItemEventArgs(item));
		}


		/// <summary>
		/// Создает и добавляет новый элемент в конец списка реестра по заданным параметрам.
		/// </summary>
		public void Add(
			string key,
			string value,
			int level,
			bool isLabel)
		{
			var item1 = new RegistryItem(key, value, level, isLabel);
			Add(item1);
		}


		/// <summary>
		/// Добавляет в реестр текстовую метку (заголовок разделов).
		/// </summary>
		/// <param name="title">Текст заголовка метки.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddLabel(
			string title)
		{
			Add(string.Empty, title, 0, true);
		}


		/// <summary>
		/// Вставляет на нулевую позицию специальный системный элемент "Нулевое/пустое значение".
		/// </summary>
		/// <param name="key">Опциональный уникальный ключ элемента.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddNullItem(
			string? key = null)
		{
			Insert(0, new RegistryItem(
				key ?? string.Empty,
				Resources.Common.Text_EmptyItem,
				0, false));
		}


		/// <summary>
		/// Вставляет на нулевую позицию специальный системный элемент "Все значения".
		/// </summary>
		/// <param name="key">Опциональный уникальный ключ элемента.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddAllItems(
			string? key = null)
		{
			Insert(0, new RegistryItem(
				key ?? string.Empty,
				Resources.Common.Text_AllItems,
				0, false));
		}


		/// <summary>
		/// Выполняет локализацию элементов текущего реестра на основе другой строки сериализации.
		/// </summary>
		/// <param name="source">Строка сериализации реестра-источника локализации.</param>
		public void Localization(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return;
			var reg1 = new RegistryList(string.Empty, source);
			if (reg1.HasItems)
				foreach (var item1 in _items)
				{
					var s1 = reg1.GetValue(item1.Key);
					if (!string.IsNullOrEmpty(s1))
						item1.Value = s1;
				}
		}


		/// <summary>
		/// Извлекает каскадные метаданные поля из ресурсов и применяет их для локализации элементов реестра.
		/// </summary>
		/// <param name="key">Ключ поля в ресурсах.</param>
		/// <param name="resources">Список дополнительных менеджеров ресурсов.</param>
		public void Localization(
			string key,
			params ResourceManager[] resources)
		{
			var helper1 = new ResourcesHelper(resources);
			var face1 = helper1.GetCrudFace(key);
			if (face1 != null)
				Localization(face1.ToString());
		}


		/* functions */


		/// <summary>
		/// Сериализует весь список реестра в единую строку с разделителями ";".
		/// </summary>
		/// <returns>Строка сериализованных элементов реестра.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Join(";", _items);
		}


		/// <summary>
		/// Возвращает элемент реестра по его строковому ключу.
		/// </summary>
		/// <param name="key">Строковый ключ для поиска.</param>
		/// <returns>Найденный элемент или null, если элемент не найден.</returns>
		public RegistryItem? GetItem(
			string? key)
		{
			if (key == null)
				return null;
			return _items.FirstOrDefault(
				x => x.Key.Equals(key, StringComparison.Ordinal));
		}


		/// <summary>
		/// Возвращает элемент реестра по его целочисленному ключу.
		/// </summary>
		/// <param name="key">Целочисленный ключ для поиска.</param>
		/// <returns>Найденный элемент или null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RegistryItem? GetItem(
			int key)
		{
			return GetItem(key.ToString());
		}


		/// <summary>
		/// Возвращает элемент реестра по его целочисленному nullable-ключу.
		/// </summary>
		/// <param name="key">Nullable целочисленный ключ для поиска.</param>
		/// <returns>Найденный элемент или null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RegistryItem? GetItem(
			int? key)
		{
			return GetItem(key?.ToString());
		}


		/// <summary>
		/// Возвращает строковый ключ первого элемента, значение которого совпадает с переданным.
		/// </summary>
		/// <param name="value">Значение для поиска ключа.</param>
		/// <returns>Строковый ключ элемента или null, если значение не найдено.</returns>
		public string? GetKey(
			string value)
		{
			if (value == null)
				return null;
			var item1 = _items.FirstOrDefault(
				x => x.Value.Equals(value, StringComparison.Ordinal));
			return item1?.Key;
		}


		/// <summary>
		/// Возвращает ключ элемента по его значению с автогенерацией ключа при его отсутствии.
		/// </summary>
		/// <param name="value">Значение элемента.</param>
		/// <param name="newKey">Опциональный новый ключ.</param>
		/// <returns>Финальный строковый ключ, под которым зарегистрировано значение.</returns>
		public string GetKeyForValue(
			string value,
			string? newKey = null)
		{
			var key1 = GetKey(value);
			if (key1 != null)
				return key1;
			var actualKey1 = newKey ?? _items.Count.ToString();
			Add(actualKey1, value, 0, false);
			return actualKey1;
		}


		/// <summary>
		/// Возвращает строковое значение элемента по его строковому ключу.
		/// </summary>
		/// <param name="key">Строковый ключ элемента.</param>
		/// <returns>Значение элемента или null, если ключ отсутствует.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? GetValue(
			string key)
		{
			var item1 = GetItem(key);
			return item1?.Value;
		}


		/// <summary>
		/// Возвращает строковое значение элемента по его целочисленному ключу.
		/// </summary>
		/// <param name="key">Целочисленный ключ элемента.</param>
		/// <returns>Значение элемента или null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? GetValue(
			int key)
		{
			return GetValue(key.ToString());
		}


		/// <summary>
		/// Возвращает строковое значение элемента по его целочисленному nullable-ключу.
		/// </summary>
		/// <param name="key">Nullable целочисленный ключ элемента.</param>
		/// <returns>Значение элемента или null.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string? GetValue(
			int? key)
		{
			return GetValue(key?.ToString() ?? string.Empty);
		}


		/// <summary>
		/// Возвращает значение по ключу. Если значение не найдено, возвращает ключ, оформленный по шаблону декоратора.
		/// </summary>
		/// <param name="key">Строковый ключ для поиска.</param>
		/// <returns>Значение или декорированный ключ.</returns>
		public string GetValueOrKey(
			string key)
		{
			var item1 = GetItem(key);
			return item1?.Value ?? key.Make("–{0}–");
		}


		/// <summary>
		/// Возвращает значение по целочисленному ключу с декоратором.
		/// </summary>
		/// <param name="key">Целочисленный ключ для поиска.</param>
		/// <returns>Значение или декорированный ключ.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetValueOrKey(
			int key)
		{
			return GetValueOrKey(key.ToString());
		}


		/// <summary>
		/// Возвращает уровень вложенности элемента по его строковому ключу.
		/// </summary>
		/// <param name="key">Строковый ключ элемента.</param>
		/// <returns>Уровень вложенности от 0 до 9, либо 0, если элемент не найден.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetLevel(
			string key)
		{
			var item1 = GetItem(key);
			return item1?.Level ?? 0;
		}


		/// <summary>
		/// Возвращает уровень вложенности элемента по его целочисленному ключу.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetLevel(
			int key)
		{
			return GetLevel(key.ToString());
		}


		/// <summary>
		/// Возвращает уровень вложенности элемента по его целочисленному nullable-ключу.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetLevel(
			int? key)
		{
			return !key.HasValue
				? 0 : GetLevel(key.Value);
		}


		/// <summary>
		/// Возвращает значение первого элемента, чей ключ содержит в себе указанную подстроку.
		/// </summary>
		/// <param name="inclusion">Подстрока, которая должна входить в состав ключа.</param>
		/// <returns>Значение элемента или null.</returns>
		public string? GetValueInclusion(
			string inclusion)
		{
			if (string.IsNullOrEmpty(inclusion))
				return null;
			var item1 = _items.FirstOrDefault(
				x => x.Key.Contains(inclusion, StringComparison.Ordinal));
			return item1?.Value;
		}


		/// <summary>
		/// Возвращает значение первого элемента, чей ключ целиком входит
		/// в состав переданной большой строки расширения.
		/// </summary>
		/// <param name="expansion">Строка расширения, в которой производится поиск ключа.</param>
		/// <returns>Значение элемента или null.</returns>
		public string? GetValueExpansion(
			string expansion)
		{
			if (string.IsNullOrEmpty(expansion))
				return null;
			var item1 = _items.FirstOrDefault(
				x => expansion.Contains(x.Key, StringComparison.Ordinal));
			return item1?.Value;
		}


		/// <summary>
		/// Рассчитывает и возвращает максимальную длину значений элементов реестра
		/// с учетом фиксированного базового отступа.
		/// </summary>
		/// <returns>Максимальная длина значений плюс базовый отступ.</returns>
		public int GetMaxWidth()
		{
			const int basePadding = 9;
			if (_items.Count == 0)
				return basePadding;
			int maxLen1 = 1;
			for (int i1 = 0; i1 < _items.Count; i1++)
			{
				int currentLen1 = _items[i1].Value?.Length ?? 1;
				if (currentLen1 > maxLen1)
					maxLen1 = currentLen1;
			}
			return maxLen1 + basePadding;
		}


		/// <summary>
		/// Возвращает рекомендуемый режим отображения реестра в интерфейсе
		/// на основе расчетной ширины и общего количества элементов.
		/// </summary>
		/// <returns>Рекомендуемый режим отображения.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RegistryModeEnum GetProposeMode()
		{
			return GetProposeWidth() switch
			{
				WidthsEnum.Full => _items.Count < 10
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select,
				WidthsEnum.Large => _items.Count < 20
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select,
				WidthsEnum.Medium => _items.Count < 30
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select,
				_ => _items.Count < 40
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select
			};
		}


		/// <summary>
		/// Возвращает рекомендуемую ширину элемента ввода для отображения реестра
		/// на основе переданного числового значения ширины.
		/// </summary>
		/// <param name="width">Числовое значение максимальной ширины поля (включая отступы).</param>
		/// <returns>Рекомендуемый вариант ширины.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static WidthsEnum GetProposeWidth(
			int width)
		{
			return width switch
			{
				< 11 => WidthsEnum.Small,
				< 41 => WidthsEnum.Medium,
				< 101 => WidthsEnum.Large,
				_ => WidthsEnum.Full,
			};
		}


		/// <summary>
		/// Возвращает рекомендуемую ширину элемента ввода для отображения реестра
		/// на основе анализа максимальной длины значений текущего списка.
		/// </summary>
		/// <returns>Рекомендуемый вариант ширины.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public WidthsEnum GetProposeWidth()
		{
			return GetProposeWidth(GetMaxWidth());
		}

	}

}
