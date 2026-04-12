namespace Ans.Net10.Common
{

	public class DictTagStylers
		: Dictionary<string, TagStyler>
	{

		/* ctors */


		public DictTagStylers(
			params string[] serialization)
		{
			foreach (var item1 in serialization)
			{
				var a1 = item1.SplitFix("|", 3);
				Add(a1[0], new TagStyler(a1[1], a1[2]));
			}
		}

	}

}
