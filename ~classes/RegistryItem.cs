// rev 2026-09-20

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Представляет элемент реестра (запись), поддерживающий иерархические уровни вложенности от 0 до 9, 
	/// маркеры меток и кастомную текстовую сериализацию/десериализацию.
	/// </summary>
	public class RegistryItem
	{

		public const string MASK_semicolon = "[x3B]";
		public const string MASK_equally = "[x3D]";
		public const string MASK_sharp = "[x23]";
		public const string MASK_colon = "[x3A]";


		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр класса <see cref="RegistryItem"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RegistryItem()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="RegistryItem"/> с заданными параметрами.
		/// </summary>
		/// <param name="key">Уникальный ключ элемента.</param>
		/// <param name="value">Значение элемента.</param>
		/// <param name="level">Уровень вложенности элемента в иерархии реестра (строго от 0 до 9).</param>
		/// <param name="isLabel">Признак того, является ли элемент текстовой меткой (заголовком).</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RegistryItem(
			string key,
			string value,
			int level,
			bool isLabel)
			: this()
		{
			Key = key;
			Value = value;
			Level = level;
			IsLabel = isLabel;
		}


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="RegistryItem"/>,
		/// десериализуя его из строки формата "key=[#][:level]value".
		/// </summary>
		/// <param name="serialization">Строка сериализованных данных элемента.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RegistryItem(
			string serialization)
			: this()
		{
			FillFromString(serialization);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает уникальный ключ элемента реестра.
		/// </summary>
		public string Key { get; private set; } = string.Empty;


		/// <summary>
		/// Возвращает признак того, является ли данный элемент реестра меткой (заголовком).
		/// </summary>
		public bool IsLabel { get; private set; }


		/// <summary>
		/// Возвращает уровень вложенности элемента в структуре реестра (от 0 до 9).
		/// </summary>
		public int Level { get; private set; }


		/* properties */


		/// <summary>
		/// Получает или задает строковое значение элемента.
		/// Если передано пустое значение, автоматически подставляется имя ключа в фигурных скобках.
		/// </summary>
		public string Value
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => field = string.IsNullOrEmpty(value)
				? $"{{{Key}}}" : value;
		} = string.Empty;


		/* functions */


		/// <summary>
		/// Экранирует служебные символы (; = # :) в строке данных, добавляя перед ними обратный слэш.
		/// </summary>
		/// <param name="data">Исходная строка данных.</param>
		/// <returns>Строка данных с экранированными служебными символами.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetDataEscapes(
			string data)
		{
			if (string.IsNullOrEmpty(data))
				return string.Empty;
			return data
				.Replace(";", "\\;")
				.Replace("=", "\\=")
				.Replace("#", "\\#")
				.Replace(":", "\\:");
		}


		/// <summary>
		/// Маскирует экранированные служебные символы.
		/// </summary>
		/// <param name="data">Исходная строка данных.</param>
		/// <returns>Замаскированная строка данных.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetDataMasking(
			string data)
		{
			if (string.IsNullOrEmpty(data))
				return string.Empty;
			return data
				.Replace("\\;", MASK_semicolon)
				.Replace("\\=", MASK_equally)
				.Replace("\\#", MASK_sharp)
				.Replace("\\:", MASK_colon);
		}


		/// <summary>
		/// Восстанавливает замаскированные символы, заменяя их на оригинальные служебные символы.
		/// </summary>
		/// <param name="data">Замаскированная строка данных.</param>
		/// <returns>Восстановленная строка с неэкранированными служебными символами.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetDataRestores(
			string data)
		{
			if (string.IsNullOrEmpty(data))
				return string.Empty;
			return data
				.Replace(MASK_semicolon, ";")
				.Replace(MASK_equally, "=")
				.Replace(MASK_sharp, "#")
				.Replace(MASK_colon, ":");
		}


		/// <summary>
		/// Сериализует текущий элемент реестра в строку формата "key=[#][:level]value" с экранированием служебных символов.
		/// </summary>
		/// <returns>Строка сериализованных данных элемента.</returns>
		public override string ToString()
		{
			var key1 = GetDataEscapes(Key);
			var value1 = GetDataEscapes(Value);
			var label1 = IsLabel.Make("#");
			var level1 = (Level is > 0 and < 10).Make($":{Level}");
			return $"{key1}={label1}{level1}{value1}";
		}


		/* methods */


		/// <summary>
		/// Заполняет свойства текущего элемента реестра на основе строки сериализации.
		/// </summary>
		/// <param name="serialization">Строка данных формата "key=[#][:level]value".</param>
		/// <exception cref="ArgumentException">Вызывается, если переданная строка пуста или имеет неверный формат.</exception>
		public void FillFromString(
			string serialization)
		{
			if (string.IsNullOrWhiteSpace(serialization))
				throw new ArgumentException(
					"The serialization string cannot be empty.",
					nameof(serialization));
			var s1 = GetDataMasking(serialization);
			var i1 = s1.IndexOf('=');
			if (i1 < 1)
				throw new ArgumentException(
					"Invalid format of the registry element serialization string. The '=' character is missing.",
					nameof(serialization));
			Key = GetDataRestores(s1[..i1]);
			var s2 = s1[(i1 + 1)..];
			if (s2.Length > 0 && s2[0] == '#')
			{
				IsLabel = true;
				s2 = s2[1..];
			}
			if (s2.Length > 1 && s2[0] == ':' && char.IsDigit(s2[1]))
			{
				Level = s2[1].ToString().ToInt(0);
				s2 = s2[2..];
			}
			else if (s2.Length > 0 && s2[0] == ':')
			{
				s2 = s2[1..];
			}
			Value = GetDataRestores(s2);
		}

	}

}
