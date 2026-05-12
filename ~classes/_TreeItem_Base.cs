namespace Ans.Net10.Common
{

	public interface ITreeItem
	{
		ITreeItem Parent { get; set; }

		IEnumerable<ITreeItem> Parents { get; }
		IEnumerable<ITreeItem> Childs { get; }
		bool HasParent { get; }
		bool HasChilds { get; }

		void AppendChild(ITreeItem item);
		void AppendChilds(params ITreeItem[] items);
	}



	public class _TreeItem_Base
		: ITreeItem
	{

		private List<ITreeItem> _childs;


		/* properties */


		public ITreeItem Parent { get; set; }


		/* readonly properties */


		public IEnumerable<ITreeItem> Parents => field ?? _getParents();
		public IEnumerable<ITreeItem> Childs => _childs;
		public bool HasChilds => _childs?.Count > 0;
		public bool HasParent => Parent != null;


		/* methods */


		public void AppendChild(
			ITreeItem item)
		{
			if (item == this)
				throw new ArgumentException("An object cannot be its own Child.");
			ITreeItem temp1 = this;
			while (temp1 != null)
			{
				if (temp1 == item)
					throw new InvalidOperationException("The detected loop: this object is already a Parent in the chain above, the object cannot be its own Child.");
				temp1 = temp1.Parent;
			}
			_childs ??= [];
			_childs.Add(item);
			item.Parent = this;
		}


		public void AppendChilds(
			params ITreeItem[] items)
		{
			if (items != null)
				foreach (var item1 in items)
					AppendChild(item1);
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
