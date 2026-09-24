// rev 2026-09-18

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Специализированный словарь с целочисленным ключом (<see cref="int"/>)
	/// и строковым значением (<see cref="string"/>),
	/// поддерживающий сериализацию в текстовый формат.
	/// </summary>
	public class DictInt
		: _Dict_Proto<int, string>
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр словаря.
		/// </summary>
		public DictInt()
			: base()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря на основе перечисления сериализованных строк пар.
		/// </summary>
		public DictInt(
			IEnumerable<string> serialization)
			: base(serialization)
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря на основе массива сериализованных строк пар.
		/// </summary>
		public DictInt(
			params string[] serialization)
			: base(serialization)
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря из единой сериализованной строки.
		/// </summary>
		public DictInt(
			string serialization)
			: base(serialization)
		{
		}


		/* overrides */


		/// <inheritdoc />
		/// <exception cref="FormatException">
		/// Вызывается, если строку невозможно преобразовать в целочисленный ключ.
		/// </exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int StringToKey(
			string key)
		{
			return key.ToInt()
				?? throw new FormatException(
					$"Failed to convert the string '{key}' to an integer key of type Int32.");
		}


		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string StringToValue(
			string value)
		{
			return value;
		}


		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string KeyToString(
			int key)
		{
			return key.ToString();
		}


		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ValueToString(
			string value)
		{
			return value;
		}

	}

}
