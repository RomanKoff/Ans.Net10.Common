namespace Ans.Net10.Common
{

	public class RegistryGroup
	{

		/* ctors */


		public RegistryGroup(
			string title)
		{
			Title = title;
		}


		/* readonly properties */


		public string Title { get; }
		public List<RegistryItem> Items { get; } = [];

	}

}
