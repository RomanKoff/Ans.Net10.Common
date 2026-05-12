namespace Ans.Net10.Common
{

	public class RegistryGroup
	{
		public RegistryGroup(
			string title)
		{
			Title = title;
		}

		public string Title { get; }
		public List<RegistryItem> Items { get; } = [];
	}

}
