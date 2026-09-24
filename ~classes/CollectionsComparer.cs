// rev 2026-09-18

namespace Ans.Net10.Common
{

	/// <summary>
	/// Компаратор для детального сравнения и синхронизации двух коллекций объектов по ключевому полю.
	/// Разделяет элементы на добавленные, удаленные, актуальные и измененные.
	/// </summary>
	/// <typeparam name="TCurrent">Тип элементов в текущей (исходной) коллекции.</typeparam>
	/// <typeparam name="TNewest">Тип элементов в новой (целевой) коллекции.</typeparam>
	/// <typeparam name="TKey">Тип уникального ключа элементов.</typeparam>
	public class CollectionsComparer<TCurrent, TNewest, TKey>
		where TKey : notnull
	{

		/* ctors */


		/// <summary>
		/// Инициализирует новый экземпляр компаратора и сразу выполняет сопоставление коллекций.
		/// </summary>
		/// <param name="current">Текущая коллекция элементов.</param>
		/// <param name="newest">Новая коллекция элементов.</param>
		/// <param name="currentKeySelector">Функция извлечения ключа из текущего элемента.</param>
		/// <param name="newestKeySelector">Функция извлечения ключа из нового элемента.</param>
		/// <param name="funcDataDiff">Функция проверки изменений (возвращает true, если данные объектов с одинаковыми ключами различаются).</param>
		public CollectionsComparer(
			IEnumerable<TCurrent>? current,
			IEnumerable<TNewest>? newest,
			Func<TCurrent, TKey> currentKeySelector,
			Func<TNewest, TKey> newestKeySelector,
			Func<TCurrent, TNewest, bool> funcDataDiff)
		{
			var dictCurrents1 = (current ?? []).ToDictionary(currentKeySelector);
			var dictNewests1 = (newest ?? []).ToDictionary(newestKeySelector);
			CurrentCount = dictCurrents1.Count;
			NewestCount = dictNewests1.Count;
			var addeds1 = new List<TNewest>();
			var deleteds1 = new List<TCurrent>();
			var actualCurrents1 = new List<TCurrent>();
			var actualNewests1 = new List<TNewest>();
			var changeds1 = new List<(TCurrent Current, TNewest Newest)>();
			foreach (var currentPair1 in dictCurrents1)
				if (dictNewests1.TryGetValue(currentPair1.Key, out var newestItem1))
				{
					actualCurrents1.Add(currentPair1.Value);
					actualNewests1.Add(newestItem1);
					if (funcDataDiff(currentPair1.Value, newestItem1))
						changeds1.Add((currentPair1.Value, newestItem1));
				}
				else
					deleteds1.Add(currentPair1.Value);
			foreach (var newestPair1 in dictNewests1)
				if (!dictCurrents1.ContainsKey(newestPair1.Key))
					addeds1.Add(newestPair1.Value);
			Added = addeds1;
			Deleted = deleteds1;
			ActualCurrent = actualCurrents1;
			ActualNewest = actualNewests1;
			Changed = changeds1;
			AddedCount = addeds1.Count;
			DeletedCount = deleteds1.Count;
			ActualCount = actualCurrents1.Count;
			ChangedCount = changeds1.Count;
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает общее количество элементов в исходной коллекции.
		/// </summary>
		public int CurrentCount { get; }

		/// <summary>
		/// Возвращает общее количество элементов в новой коллекции.
		/// </summary>
		public int NewestCount { get; }

		/// <summary>
		/// Возвращает коллекцию добавленных элементов, которые появились
		/// в новой коллекции, но отсутствовали в текущей.
		/// </summary>
		public IReadOnlyCollection<TNewest> Added { get; }

		/// <summary>
		/// Возвращает количество добавленных элементов.
		/// </summary>
		public int AddedCount { get; }

		/// <summary>
		/// Возвращает признак наличия добавленных элементов.
		/// </summary>
		public bool HasAdded => AddedCount > 0;

		/// <summary>
		/// Возвращает коллекцию удаленных элементов, которые присутствовали
		/// в текущей коллекции, но отсутствуют в новой.
		/// </summary>
		public IReadOnlyCollection<TCurrent> Deleted { get; }

		/// <summary>
		/// Возвращает количество удаленных элементов.
		/// </summary>
		public int DeletedCount { get; }

		/// <summary>
		/// Возвращает признак наличия удаленных элементов.
		/// </summary>
		public bool HasDeleted
			=> DeletedCount > 0;

		/// <summary>
		/// Возвращает элементы из текущей коллекции, которые сохранили
		/// свое присутствие в новой коллекции.
		/// </summary>
		public IReadOnlyCollection<TCurrent> ActualCurrent { get; }

		/// <summary>
		/// Возвращает элементы из новой коллекции, которые уже
		/// существовали в текущей коллекции.
		/// </summary>
		public IReadOnlyCollection<TNewest> ActualNewest { get; }

		/// <summary>
		/// Возвращает количество актуальных (совпадающих по ключам) элементов.
		/// </summary>
		public int ActualCount { get; }

		/// <summary>
		/// Возвращает признак наличия актуальных элементов.
		/// </summary>
		public bool HasActual
			=> ActualCount > 0;

		/// <summary>
		/// Возвращает коллекцию пар элементов, у которых совпадают ключи,
		/// но внутренние данные различаются по результатам проверки.
		/// </summary>
		public IReadOnlyCollection<(TCurrent Current, TNewest Newest)> Changed { get; }

		/// <summary>
		/// Возвращает количество измененных элементов.
		/// </summary>
		public int ChangedCount { get; }

		/// <summary>
		/// Возвращает признак наличия измененных элементов.
		/// </summary>
		public bool HasChanged
			=> ChangedCount > 0;


		/* methods */


		/// <summary>
		/// Выводит диагностическую и отладочную информацию о результатах сравнения коллекций в консоль.
		/// </summary>
		public void TestDebug()
		{
			Console.WriteLine("[Ans.Net10.Common] CollectionsComparer.TestDebug()");
			Console.WriteLine($"   Current: {CurrentCount}");
			Console.WriteLine($"   Newest: {NewestCount}");
			Console.WriteLine($"   Added: {AddedCount}");
			Console.WriteLine($"   Deleted: {DeletedCount}");
			Console.WriteLine($"   Actual: {ActualCount}");
			Console.WriteLine($"   Changed: {ChangedCount}");
		}

	}

}
