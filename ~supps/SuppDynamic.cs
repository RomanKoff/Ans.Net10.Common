using System.Dynamic;

namespace Ans.Net10.Common
{

	/*
	 *	For Razor use ViewData.Get...
	 */

	public static class SuppDynamic
	{

		/* functions */


		public static bool HasProperty(
			dynamic item,
			string propertyName)
		{
			if (item is ExpandoObject obj1)
				return (obj1 as IDictionary<string, object>).ContainsKey(propertyName);
			return item.GetType().GetProperty(propertyName);
		}

	}

}
