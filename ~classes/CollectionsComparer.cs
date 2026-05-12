namespace Ans.Net10.Common
{

	public class CollectionsComparer<TCurrent, TNewest, TKey>
	{

		/* ctors */


		public CollectionsComparer(
			IEnumerable<TCurrent> current,
			IEnumerable<TNewest> newest,
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
			var changeds1 = new List<(TCurrent, TNewest)>();

			foreach (var current1 in dictCurrents1)
			{
				if (dictNewests1.TryGetValue(current1.Key, out var newest1))
				{
					actualCurrents1.Add(current1.Value);
					actualNewests1.Add(newest1);
					if (funcDataDiff(current1.Value, newest1))
						changeds1.Add((current1.Value, newest1));
				}
				else
					deleteds1.Add(current1.Value);
			}

			foreach (var newest1 in dictNewests1)
				if (!dictCurrents1.ContainsKey(newest1.Key))
					addeds1.Add(newest1.Value);

			Added = addeds1;
			Deleted = deleteds1;
			ActualCurrent = actualCurrents1;
			ActualNewest = actualNewests1;
			Changed = changeds1;
		}


		/* readonly properties */


		public int CurrentCount { get; }
		public int NewestCount { get; }

		/// <summary>
		/// Добавленные новые элементы
		/// </summary>
		public IEnumerable<TNewest> Added { get; }
		public int AddedCount => Added?.Count() ?? 0;
		public bool HasAdded => AddedCount > 0;

		/// <summary>
		/// Удаленные текущие элементы
		/// </summary>
		public IEnumerable<TCurrent> Deleted { get; }
		public int DeletedCount => Deleted?.Count() ?? 0;
		public bool HasDeleted => DeletedCount > 0;

		/// <summary>
		/// Актуальные текущие (есть и в текущей и в новой коллекции)
		/// </summary>
		public IEnumerable<TCurrent> ActualCurrent { get; }

		/// <summary>
		/// Актуальные новые (есть и в новой коллекции и в текущей)
		/// </summary>
		public IEnumerable<TNewest> ActualNewest { get; }

		public int ActualCount => ActualCurrent?.Count() ?? 0;
		public bool HasActual => ActualCount > 0;

		/// <summary>
		/// Измененные (одинаковые ключи, но разное содержимое)
		/// </summary>
		public IEnumerable<(TCurrent Current, TNewest Newest)> Changed { get; }
		public int ChangedCount => Changed?.Count() ?? 0;
		public bool HasChanged => ChangedCount > 0;


		/* methods */


		public void TestDebug()
		{
			Console.WriteLine($"[Ans.Net10.Common] CollectionsComparer.TestDebug()");
			Console.WriteLine($"   Current: {CurrentCount}");
			Console.WriteLine($"   Newest: {NewestCount}");
			Console.WriteLine($"   Added: {AddedCount}");
			Console.WriteLine($"   Deleted: {DeletedCount}");
			Console.WriteLine($"   Actual: {ActualCount}");
			Console.WriteLine($"   Changed: {ChangedCount}");
		}

	}

}
