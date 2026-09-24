// rev 2026-09-18

using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Абстрактный базовый класс для словарей, поддерживающих кастомную текстовую сериализацию
	/// с поддержкой экранирования разделителей ключей (=) и пар (;).
	/// </summary>
	/// <typeparam name="TKey">Тип ключа словаря.</typeparam>
	/// <typeparam name="TValue">Тип значения словаря.</typeparam>
	public abstract class _Dict_Proto<TKey, TValue>
		: Dictionary<TKey, TValue>, IDictionary<TKey, TValue>
		where TKey : notnull
	{

		/* abstracts */


		/// <summary>
		/// Преобразует строковое представление ключа в целевой тип <typeparamref name="TKey"/>.
		/// </summary>
		public abstract TKey StringToKey(string key);


		/// <summary>
		/// Преобразует строковое представление значения в целевой тип <typeparamref name="TValue"/>.
		/// </summary>
		public abstract TValue StringToValue(string value);


		/// <summary>
		/// Преобразует ключ типа <typeparamref name="TKey"/> в строковое представление.
		/// </summary>
		public abstract string KeyToString(TKey key);


		/// <summary>
		/// Преобразует значение типа <typeparamref name="TValue"/> в строковое представление.
		/// </summary>
		public abstract string ValueToString(TValue value);


		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр словаря.
		/// </summary>
		protected _Dict_Proto()
			: base()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря на основе коллекции сериализованных строк пар.
		/// </summary>
		protected _Dict_Proto(
			IEnumerable<string>? serialization)
			: this()
		{
			if (serialization != null && serialization.Any())
				AddItems(serialization);
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря из общей сериализованной строки (например, "k1=v1;k2=v2").
		/// </summary>
		protected _Dict_Proto(
			string? serialization)
			: this()
		{
			if (string.IsNullOrEmpty(serialization))
				return;
			var parts1 = serialization.Split(';');
			var currentItem1 = new StringBuilder();
			foreach (var part1 in parts1)
			{
				if (currentItem1.Length > 0)
					currentItem1.Append(';');
				currentItem1.Append(part1);
				if (part1.EndsWith('\\'))
				{
					currentItem1.Length--;
					continue;
				}
				Add(currentItem1.ToString());
				currentItem1.Clear();
			}
		}


		/* methods */


		/// <summary>
		/// Добавляет в словарь пару ключ-значение из одной сериализованной строки вида "ключ=значение".
		/// </summary>
		public void Add(
			string serialization)
		{
			if (string.IsNullOrEmpty(serialization))
				return;
			var pair = serialization.Split('=', 2);
			if (pair.Length < 2)
				return;
			var rawKey1 = pair[0]
				.Replace("\\=", "=")
				.Replace("\\;", ";");
			var rawValue1 = pair[1]
				.Replace("\\=", "=")
				.Replace("\\;", ";");
			Add(StringToKey(rawKey1), StringToValue(rawValue1));
		}


		/// <summary>
		/// Массово добавляет элементы из коллекции сериализованных строк.
		/// </summary>
		public void AddItems(
			IEnumerable<string> serialization)
		{
			foreach (var item1 in serialization)
				Add(item1);
		}


		/* functions */


		/// <summary>
		/// Сериализует текущий словарь в единую строку с экранированием служебных символов.
		/// </summary>
		public override string ToString()
		{
			if (Count == 0)
				return string.Empty;
			var sb1 = new StringBuilder();
			foreach (var key1 in Keys)
			{
				var escapedKey = KeyToString(key1)
					.Replace("=", "\\=")
					.Replace(";", "\\;");
				var escapedValue = ValueToString(this[key1])
					.Replace("=", "\\=")
					.Replace(";", "\\;");
				sb1
					.Append(escapedKey)
					.Append('=')
					.Append(escapedValue)
					.Append(';');
			}

			return sb1.ToString()[..^1];
		}


		/// <summary>
		/// Безопасно возвращает значение по ключу. Если ключ отсутствует, пытается 
		/// преобразовать имя ключа в тип значения.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TValue? GetValueOrKey(
			TKey key)
		{
			if (key == null)
				return default;
			return TryGetValue(key, out var value1)
				? value1 : StringToValue(KeyToString(key));
		}

	}

}
