using System.Resources;

namespace Ans.Net10.Common
{

	public enum RegistryModeEnum
	{
		Auto,
		Inputs,
		Select
	}



	public enum WidthEnum
	{
		Nothing,
		ExtraSmall,
		Small,
		Medium,
		Large,
		ExtraLarge,
		Full,
	}



	public class RegistryItemEventArgs
		: EventArgs
	{
		public RegistryItem Item { get; private set; }
		public RegistryItemEventArgs(RegistryItem item) { Item = item; }
	}



	public delegate void RegistryItemEventHandler(
		object sender,
		RegistryItemEventArgs e);



	public class RegistryList
	{

		private readonly List<RegistryItem> _items = [];


		/* ctors */


		public RegistryList()
		{
		}


		public RegistryList(
			params RegistryItem[] items)
			: this()
		{
			if (items?.Length == 0)
				_items.Clear();
			else
				_items = [.. items];
		}


		public RegistryList(
			IEnumerable<RegistryItem> items)
			: this(items?.ToArray())
		{
		}


		/// <summary>
		/// Сериализация реестра из строки
		/// </summary>
		/// <param name="serialization">
		/// Строка сериализации реестра в виде
		/// "item1;item2;...".
		/// Символы ';=#:' экранируются '\'
		/// </param>
		public RegistryList(
			string serialization)
			: this()
		{
			FillFromString(serialization);
		}


		public RegistryList(
			params string[] items)
			: this()
		{
			FillFromString(items);
		}


		/* events */


		public event RegistryItemEventHandler AddedItem;


		/* readonly properties */


		public IEnumerable<RegistryItem> Items
			=> _items;


		public bool HasItems
			=> _items?.Count > 0;


		/* methods */


		protected void OnAddedItem(
			RegistryItemEventArgs e)
		{
			AddedItem?.Invoke(this, e);
		}


		public void Add(
			RegistryItem item)
		{
			_items.Add(item);
			OnAddedItem(new RegistryItemEventArgs(item));
		}


		public void Insert(
			int index,
			RegistryItem item)
		{
			_items.Insert(index, item);
			OnAddedItem(new RegistryItemEventArgs(item));
		}


		public void Add(
			string key,
			string value,
			int level,
			bool isLabel)
		{
			var item1 = new RegistryItem(key, value, level, isLabel);
			Add(item1);
		}


		public void AddLabel(
			string title)
		{
			Add(null, title, 0, true);
		}


		public void AddNullItem(
			string key = null)
		{
			Insert(0, new RegistryItem(
				key ?? string.Empty,
				Resources.Common.Text_EmptyItem,
				0, false));
		}


		public void AddAllItems(
			string key = null)
		{
			Insert(0, new RegistryItem(
				key ?? string.Empty,
				Resources.Common.Text_AllItems,
				0, false));
		}


		/// <summary>
		/// Десериализация реестра из строки
		/// </summary>
		/// <param name="items">
		/// "!" — добавляет элемент «нулевое значение».
		/// "*" — добавляет элемент «все значения».
		/// Символы ';=#:' экранируются '\'
		/// </param>
		public void FillFromString(
			params string[] items)
		{
			_items.Clear();
			if (items?.Length != 0)
			{
				bool hasNull1 = false;
				bool hasAll1 = false;
				foreach (var item1 in items)
					if (!hasNull1 && item1 == "!")
					{
						AddNullItem();
						hasNull1 = true;
					}
					else if (!hasAll1 && item1 == "*")
					{
						AddAllItems();
						hasAll1 = true;
					}
					else
						Add(new RegistryItem(item1));
			}
		}


		/// <summary>
		/// Сериализация реестра из строки
		/// </summary>
		/// <param name="serialization">
		/// Строка сериализации реестра в виде
		/// "item1;item2;...".
		/// Символы ';=#:' экранируются '\'
		/// </param>
		public void FillFromString(
			string serialization)
		{
			FillFromString(serialization?.Split(';'));
		}


		public void Localization(
			string source)
		{
			if (!string.IsNullOrEmpty(source))
			{
				var reg1 = new RegistryList(source);
				if (reg1.Items?.Count() > 0)
					foreach (var item1 in _items)
					{
						var s1 = reg1.GetValue(item1.Key);
						if (!string.IsNullOrEmpty(s1))
							item1.Value = s1;
					}
			}
		}


		public void Localization(
			string key,
			params ResourceManager[] resources)
		{
			var helper1 = new ResourcesHelper(resources);
			var s1 = helper1.GetCalcFaceHelper(key).GetFace();
			Localization(s1);
		}


		/* functions */


		/// <summary>
		/// Возвращает сериализацию реестра в виде строки
		/// "item1;item2;...".
		/// </summary>
		public override string ToString()
		{
			return string.Join(";", _items.Select(x => x.ToString()));
		}


		/// <summary>
		/// Возвращает элемент по ключу
		/// </summary>
		public RegistryItem GetItem(
			string key)
		{
			return _items.FirstOrDefault(
				x => x.Key?.Equals(key) ?? false);
		}


		/// <summary>
		/// Возвращает элемент по ключу
		/// </summary>
		public RegistryItem GetItem(
			int key)
		{
			return GetItem(key.ToString());
		}


		/// <summary>
		/// Возвращает элемент по ключу
		/// </summary>
		public RegistryItem GetItem(
			int? key)
		{
			return GetItem(key?.ToString());
		}


		/// <summary>
		/// Возвращает ключ по значению
		/// </summary>
		public string GetKey(
			string value)
		{
			var item1 = _items.FirstOrDefault(
				x => x.Value.Equals(value));
			return item1?.Key;
		}


		/// <summary>
		/// Возвращает ключ элемента.
		/// Если элемент отсутствует, то создает его
		/// </summary>
		public string GetKeyForValue(
			string value,
			string newKey = null)
		{
			var key1 = GetKey(value);
			if (key1 != null)
				return key1;
			Add(newKey ?? _items.Count.ToString(), value, 0, false);
			return newKey;
		}


		/// <summary>
		/// Возвращает значение по ключу
		/// </summary>
		public string GetValue(
			string key)
		{
			var item1 = GetItem(key);
			return item1?.Value;
		}


		/// <summary>
		/// Возвращает значение по ключу
		/// </summary>
		public string GetValue(
			int key)
		{
			return GetValue(key.ToString());
		}


		/// <summary>
		/// Возвращает значение по ключу
		/// </summary>
		public string GetValue(
			int? key)
		{
			return GetValue(key?.ToString());
		}


		/// <summary>
		/// Возвращает значение по ключу.
		/// Если значение не найдено, то ключ.
		/// </summary>
		public string GetValueOrKey(
			string key)
		{
			var item1 = GetItem(key);
			return item1?.Value ?? key.Make("–{0}–");
		}


		/// <summary>
		/// Возвращает значение по ключу.
		/// Если значение не найдено, то ключ.
		/// </summary>
		public string GetValueOrKey(
			int key)
		{
			return GetValue(key.ToString()) ?? key.Make("–{0}–"); ;
		}


		/// <summary>
		/// Возвращает уровень в дереве по ключу
		/// </summary>
		public int GetLevel(
			string key)
		{
			var item1 = GetItem(key);
			return item1?.Level ?? 0;
		}


		/// <summary>
		/// Возвращает уровень в дереве по ключу
		/// </summary>
		public int GetLevel(
			int key)
		{
			return GetLevel(key.ToString());
		}
		/// <summary>
		/// Возвращает уровень в дереве по ключу
		/// </summary>
		public int GetLevel(
			int? key)
		{
			return (key == null)
				? 0 : GetLevel(key.Value);
		}


		/// <summary>
		/// Возвращает значение если ключ содержит 'inclusion'
		/// </summary>
		public string GetValueInclusion(
			string inclusion)
		{
			var item1 = _items.FirstOrDefault(
				x => x.Key.IndexOf(inclusion) > 0);
			return item1?.Value;
		}


		/// <summary>
		/// Возвращает значение если 'expansion' содержит ключ
		/// </summary>
		public string GetValueExpansion(
			string expansion)
		{
			var item1 = _items.FirstOrDefault(
				x => expansion.IndexOf(x.Key) > 0);
			return item1?.Value;
		}


		/// <summary>
		/// Возвращает максимальную длину значения элемента реестра
		/// </summary>
		public int GetMaxWidth()
		{
			return _items.Max(x => x.Value?.Length ?? 1) + 2;
		}


		/// <summary>
		/// Возвращает предложение по режиму отображения реестра
		/// </summary>
		public RegistryModeEnum GetProposeMode()
		{
			return GetProposeWidth() switch
			{
				WidthEnum.Full => _items.Count < 10
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select,
				WidthEnum.Large => _items.Count < 20
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select,
				WidthEnum.Medium => _items.Count < 30
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select,
				_ => _items.Count < 40
					? RegistryModeEnum.Inputs
					: RegistryModeEnum.Select
			};
		}


		/// <summary>
		/// Возвращает предожение по ширине элемента ввода
		/// для отображение реестра
		/// </summary>
		public static WidthEnum GetProposeWidth(
			int width)
		{
			return width switch
			{
				< 11 => WidthEnum.Small,
				< 41 => WidthEnum.Medium,
				< 101 => WidthEnum.Large,
				_ => WidthEnum.Full,
			};
		}


		/// <summary>
		/// Возвращает предожение по ширине элемента ввода
		/// для отображение реестра
		/// </summary>
		public WidthEnum GetProposeWidth()
		{
			return GetProposeWidth(
				GetMaxWidth());
		}

	}

}
