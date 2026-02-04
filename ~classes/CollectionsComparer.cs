using System.Diagnostics;

namespace Ans.Net10.Common
{

	public class CollectionsComparer<TCurrent, TNewest>
	{

		/* ctor */


		public CollectionsComparer(
			IEnumerable<TCurrent> current,
			IEnumerable<TNewest> newest,
			Func<TCurrent, TNewest, bool> equalTest)
		{
			var current1 = current ?? [];
			var newest1 = newest ?? [];
			var remainingCurrent1 = new List<TCurrent>();
			var remainingNewest1 = new List<TNewest>();
			var deleting1 = new List<TCurrent>();
			var added1 = new List<TNewest>();
			bool f1;
			CurrentCount = current.Count();
			NewestCount = newest.Count();
			foreach (var item1 in current1)
			{
				f1 = false;
				foreach (var item2 in newest1)
					if (equalTest(item1, item2))
					{
						f1 = true;
						break;
					}
				if (f1)
					remainingCurrent1.Add(item1);
				else
					deleting1.Add(item1);
			}
			foreach (var item1 in newest1)
			{
				f1 = false;
				foreach (var item2 in current1)
					if (equalTest(item2, item1))
					{
						f1 = true;
						break;
					}
				if (f1)
					remainingNewest1.Add(item1);
				else
					added1.Add(item1);
			}

			AddedCount = added1.Count;
			HasAdded = AddedCount > 0;
			if (HasAdded) Added = added1;

			DeletedCount = deleting1.Count;
			HasDeleted = DeletedCount > 0;
			if (HasDeleted) Deleted = deleting1;

			RemainingCount = remainingCurrent1.Count;
			HasRemaining = RemainingCount > 0;
			if (HasRemaining)
			{
				RemainingCurrent = remainingCurrent1;
				RemainingNewest = remainingNewest1;
			}

		}


		/* readonly properties */

		public int CurrentCount { get; }
		public int NewestCount { get; }

		public IEnumerable<TNewest> Added { get; }
		public int AddedCount { get; }
		public bool HasAdded { get; }

		public IEnumerable<TCurrent> Deleted { get; }
		public int DeletedCount { get; }
		public bool HasDeleted { get; }

		public IEnumerable<TCurrent> RemainingCurrent { get; }
		public IEnumerable<TNewest> RemainingNewest { get; }
		public int RemainingCount { get; }
		public bool HasRemaining { get; }


		/* methods */


		public void TestDebug()
		{
			Debug.WriteLine($"[Ans.Net10.Common] CollectionsComparer.TestDebug()");
			Debug.WriteLine($"   Current: {CurrentCount}");
			Debug.WriteLine($"   Newest: {NewestCount}");
			Debug.WriteLine($"   Added: {AddedCount}");
			Debug.WriteLine($"   Deleted: {DeletedCount}");
			Debug.WriteLine($"   Remaining: {RemainingCount}");
		}

	}

}
