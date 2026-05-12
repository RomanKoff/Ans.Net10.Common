namespace Ans.Net10.Common
{

	public interface ITreeEntity
	{
		int Id { get; set; }
		int? ParentPtr { get; set; }
		int Order { get; set; }
	}



	public class TreeEntityWrapper<TEntity>
		where TEntity : class, ITreeEntity
	{
		public int Id { get; set; }
		public int? ParentPtr { get; set; }
		public int Order { get; set; }
		public int Level { get; set; }
		public TreeEntityWrapper<TEntity> Parent { get; set; }
		public IEnumerable<TreeEntityWrapper<TEntity>> Childs { get; set; }
		public TEntity Value { get; set; }
	}



	public class TreeEntityHelper<TEntity>
		where TEntity : class, ITreeEntity
	{

		private readonly IEnumerable<TEntity> _source;
		private readonly Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> _funcOrder;


		/* ctor */


		public TreeEntityHelper(
			IEnumerable<TEntity> source,
			int? currentId,
			Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> funcOrder)
		{
			_source = source;
			_funcOrder = funcOrder;
			_scanTable(null, 0, currentId);
		}


		/* readonly properties */


		private readonly List<TreeEntityWrapper<TEntity>> _topItems = [];
		public IEnumerable<TreeEntityWrapper<TEntity>> TopItems
			=> _topItems;

		private readonly List<TreeEntityWrapper<TEntity>> _allItems = [];
		public IEnumerable<TreeEntityWrapper<TEntity>> AllItems
			=> _allItems;


		/* privates */


		private void _scanTable(
			TreeEntityWrapper<TEntity> parent,
			int level,
			int? currentId)
		{
			var items1 = _funcOrder(
				_source.Where(
					x => x.ParentPtr == parent?.Value.Id && x.Id != currentId));
			var childs1 = new List<TreeEntityWrapper<TEntity>>();
			foreach (var item1 in items1)
			{
				var item2 = new TreeEntityWrapper<TEntity>
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
