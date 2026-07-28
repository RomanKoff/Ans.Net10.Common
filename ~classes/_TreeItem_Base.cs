namespace Ans.Net10.Common
{

	public interface ITreeItem
	{
		ITreeItem Parent { get; set; }

		IEnumerable<ITreeItem> Parents { get; }
		IEnumerable<ITreeItem> Children { get; }
		bool HasParent { get; }
		bool HasChildren { get; }

		void AppendChild(ITreeItem item);
		void AppendChildren(params ITreeItem[] items);

		T FindItem<T>(Func<T, bool> func);
	}



	public class _TreeItem_Base
		: ITreeItem
	{

		private List<ITreeItem> _children;


		/* properties */


		public ITreeItem Parent { get; set; }


		/* readonly properties */


		private List<ITreeItem> _parents;
		public IEnumerable<ITreeItem> Parents => _parents ??= [.. _getParents()];

		public IEnumerable<ITreeItem> Children => _children ?? Enumerable.Empty<ITreeItem>();
		public bool HasChildren => _children?.Count > 0;
		public bool HasParent => Parent != null;


		/* methods */


		public void AppendChild(
			ITreeItem item)
		{
			if (item == this)
				throw new ArgumentException("[Ans.Net10.Common] An object cannot be its own Child.");
			ITreeItem temp1 = this;
			while (temp1 != null)
			{
				if (temp1 == item)
					throw new InvalidOperationException("[Ans.Net10.Common] The detected loop: this object is already a Parent in the chain above, the object cannot be its own Child.");
				temp1 = temp1.Parent;
			}
			_children ??= [];
			_children.Add(item);
			item.Parent = this;
		}


		public void AppendChildren(
			params ITreeItem[] items)
		{
			if (items != null)
				foreach (var item1 in items)
					AppendChild(item1);
		}


		/* functions */


		public T FindItem<T>(
			Func<T, bool> func)
		{
			if (HasChildren)
			{
				foreach (var item1 in Children)
				{
					if (item1 is T item2 && func(item2))
						return item2;
					if (item1 is _TreeItem_Base baseItem)
					{
						T found1 = baseItem.FindItem(func);
						if (found1 != null)
							return found1;
					}
				}
			}
			return default;
		}


		/* privates */


		private IEnumerable<ITreeItem> _getParents()
		{
			var item1 = Parent;
			while (item1 != null)
			{
				yield return item1;
				item1 = item1.Parent;
			}
		}

	}

}
