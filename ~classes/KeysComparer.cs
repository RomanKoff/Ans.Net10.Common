namespace Ans.Net10.Common
{

	public class KeysComparer
	{

		/* ctors */


		public KeysComparer(
			IEnumerable<int> oldKeys,
			IEnumerable<int> newKeys)
		{
			_keysComparer(oldKeys, newKeys);
		}


		public KeysComparer(
			string[] oldKeys,
			string[] newKeys)
		{
			_keysComparer(oldKeys, newKeys);
		}


		public KeysComparer(
			string oldKeys,
			string newKeys)
		{
			string[] a1 = string.IsNullOrEmpty(oldKeys)
				? [] : oldKeys.Split(',');
			string[] a2 = string.IsNullOrEmpty(newKeys)
				? [] : newKeys.Split(',');
			_keysComparer(a1, a2);
		}


		/* readonly properties */


		public IEnumerable<int> Added { get; private set; }
		public IEnumerable<int> Deleted { get; private set; }

		public string AddedString
			=> Added?.MakeFromCollection(x => x.ToString(), null, null, ",");

		public string DeletedString
			=> Deleted?.MakeFromCollection(x => x.ToString(), null, null, ",");

		public bool HasAdded
			=> Added?.Count() > 0;

		public bool HasDeleted
			=> Deleted?.Count() > 0;


		/* privates */


		private void _keysComparer(
			IEnumerable<int> oldKeys,
			IEnumerable<int> newKeys)
		{
			if (oldKeys == null)
				Added = newKeys;
			else if (newKeys == null)
				Deleted = oldKeys;
			else
			{
				Added = newKeys.Except(oldKeys);
				Deleted = oldKeys.Except(newKeys);
			}
		}


		private void _keysComparer(
			string[] oldKeys,
			string[] newKeys)
		{
			_keysComparer(oldKeys.ToIntArray(), newKeys.ToIntArray());
		}

	}

}
