// rev 2026-09-18

namespace Ans.Net10.Common
{

	/// <summary>
	/// Компаратор для быстрого сопоставления двух списков целочисленных ключей (идентификаторов).
	/// Разделяет ключи на добавленные и удаленные.
	/// </summary>
	public class KeysComparer
	{

		/* ctors */


		/// <summary>
		/// Инициализирует компаратор на основе двух коллекций чисел.
		/// </summary>
		/// <param name="oldKeys">Исходная (старая) коллекция ключей.</param>
		/// <param name="newKeys">Целевая (новая) коллекция ключей.</param>
		public KeysComparer(
			IEnumerable<int>? oldKeys,
			IEnumerable<int>? newKeys)
		{
			_keysComparer(oldKeys, newKeys);
		}


		/// <summary>
		/// Инициализирует компаратор на основе двух массивов строковых представлений чисел.
		/// </summary>
		/// <param name="oldKeys">Исходный массив строковых ключей.</param>
		/// <param name="newKeys">Целевой массив строковых ключей.</param>
		public KeysComparer(
			string[]? oldKeys,
			string[]? newKeys)
		{
			_keysComparer(oldKeys, newKeys);
		}


		/// <summary>
		/// Инициализирует компаратор на основе двух строк с числами, разделенными запятой.
		/// </summary>
		/// <param name="oldKeys">Строка исходных ключей (например, "1,2,3").</param>
		/// <param name="newKeys">Строка целевых ключей (например, "3,4,5").</param>
		public KeysComparer(
			string? oldKeys,
			string? newKeys)
		{
			var a1 = string.IsNullOrEmpty(oldKeys)
				? [] : oldKeys.Split(',');
			var a2 = string.IsNullOrEmpty(newKeys)
				? [] : newKeys.Split(',');
			_keysComparer(a1, a2);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает коллекцию новых ключей, которые отсутствовали в исходном наборе.
		/// </summary>
		public IReadOnlyCollection<int> Added { get; private set; } = [];


		/// <summary>
		/// Возвращает коллекцию старых ключей, которые были удалены в новом наборе.
		/// </summary>
		public IReadOnlyCollection<int> Deleted { get; private set; } = [];


		/// <summary>
		/// Возвращает строку добавленных ключей, разделенных запятой.
		/// </summary>
		public string AddedString
			=> Added.MakeFromCollection(x => x.ToString(), null, null, ",");


		/// <summary>
		/// Возвращает строку удаленных ключей, разделенных запятой.
		/// </summary>
		public string DeletedString
			=> Deleted.MakeFromCollection(x => x.ToString(), null, null, ",");


		/// <summary>
		/// Возвращает признак наличия добавленных ключей.
		/// </summary>
		public bool HasAdded
			=> Added.Count > 0;


		/// <summary>
		/// Возвращает признак наличия удаленных ключей.
		/// </summary>
		public bool HasDeleted
			=> Deleted.Count > 0;


		/* privates */


		private void _keysComparer(
			IEnumerable<int>? oldKeys,
			IEnumerable<int>? newKeys)
		{
			if (oldKeys == null && newKeys == null)
			{
				Added = [];
				Deleted = [];
			}
			else if (oldKeys == null)
			{
				Added = [.. newKeys!];
				Deleted = [];
			}
			else if (newKeys == null)
			{
				Added = [];
				Deleted = [.. oldKeys];
			}
			else
			{
				Added = [.. newKeys.Except(oldKeys)];
				Deleted = [.. oldKeys.Except(newKeys)];
			}
		}


		private void _keysComparer(
			string[]? oldKeys,
			string[]? newKeys)
		{
			var oldInts = oldKeys == null
				? [] : oldKeys.ToIntArray();
			var newInts = newKeys == null
				? [] : newKeys.ToIntArray();
			_keysComparer(oldInts, newInts);
		}

	}

}
