namespace Ans.Net10.Common
{

	public interface ITreeItem
	{
		int Id { get; set; }
		int? ParentPtr { get; set; }
		int Order { get; set; }
	}



	public class TreeItemWrapper<T>
		where T : class, ITreeItem
	{
		public int Id { get; set; }
		public int? ParentPtr { get; set; }
		public int Order { get; set; }
		public int Level { get; set; }
		public TreeItemWrapper<T> Parent { get; set; }
		public IEnumerable<TreeItemWrapper<T>> Childs { get; set; }
		public T Value { get; set; }
	}



	public class TreeHelper<T>
		where T : class, ITreeItem
	{

		private readonly IEnumerable<T> _source;
		private readonly Func<IEnumerable<T>, IOrderedEnumerable<T>> _funcOrder;


		/* ctor */


		public TreeHelper(
			IEnumerable<T> source,
			int? currentId,
			Func<IEnumerable<T>, IOrderedEnumerable<T>> funcOrder)
		{
			_source = source;
			_funcOrder = funcOrder;
			_scanTable(null, 0, currentId);
		}


		/* readonly properties */


		private readonly List<TreeItemWrapper<T>> _topItems = [];
		public IEnumerable<TreeItemWrapper<T>> TopItems
			=> _topItems;

		private readonly List<TreeItemWrapper<T>> _allItems = [];
		public IEnumerable<TreeItemWrapper<T>> AllItems
			=> _allItems;


		/* privates */


		private void _scanTable(
			TreeItemWrapper<T> parent,
			int level,
			int? currentId)
		{
			var items1 = _funcOrder(
				_source.Where(
					x => x.ParentPtr == parent?.Value.Id && x.Id != currentId));
			var childs1 = new List<TreeItemWrapper<T>>();
			foreach (var item1 in items1)
			{
				var item2 = new TreeItemWrapper<T>
				{
					Id = item1.Id,
					ParentPtr = item1.ParentPtr,
					Parent = parent,
					Order = item1.Order,
					Level = level,
					Value = item1
				};
				if (parent == null)
					_topItems.Add(item2);
				else
					childs1.Add(item2);
				_allItems.Add(item2);
				_scanTable(item2, level + 1, currentId);
			}
			if (childs1.Count > 0)
				parent.Childs = childs1;
		}

	}

}
