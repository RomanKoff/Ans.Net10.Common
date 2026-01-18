namespace Ans.Net10.Common
{

	public static partial class SuppDictionary
	{

		/* functions */


		public static DictString GetDictString(
			string serialization)
		{
			if (string.IsNullOrEmpty(serialization))
				return null;
			return new DictString(serialization);
		}


		public static DictInt GetDictInt(
			string serialization)
		{
			if (string.IsNullOrEmpty(serialization))
				return null;
			return new DictInt(serialization);
		}

	}

}
