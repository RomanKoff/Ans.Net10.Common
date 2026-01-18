using System.Text;

namespace Ans.Net10.Common
{

	public static class SuppStringBuilder
	{

		/* methods */


		public static void FixSpecChars(
			StringBuilder sb)
		{
			for (int i1 = 0; i1 < sb.Length; ++i1)
				if (sb[i1] < 32)
					sb[i1] = ' ';
		}

	}

}
