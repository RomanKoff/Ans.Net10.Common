// rev 2026-09-18

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Специализированный строковый словарь (строковые ключи и значения),
	/// поддерживающий сериализацию в текстовый формат.
	/// </summary>
	public class DictString : _Dict_Proto<string, string>
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый пустой экземпляр словаря.
		/// </summary>
		public DictString()
			: base()
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря на основе перечисления сериализованных строк пар.
		/// </summary>
		public DictString(
			IEnumerable<string> serialization)
			: base(serialization)
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря на основе массива сериализованных строк пар.
		/// </summary>
		public DictString(
			params string[] serialization)
			: base(serialization)
		{
		}


		/// <summary>
		/// Инициализирует новый экземпляр словаря из единой сериализованной строки.
		/// </summary>
		public DictString(
			string serialization)
			: base(serialization)
		{
		}


		/* overrides */


		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string StringToKey(
			string key)
		{
			return key;
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
			string key)
		{
			return key;
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
